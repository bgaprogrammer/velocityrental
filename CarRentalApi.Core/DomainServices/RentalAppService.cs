using CarRentalApi.Core.Dto;
using CarRentalApi.Core.Entities;
using CarRentalApi.Core.Repositories;

namespace CarRentalApi.Core.DomainServices;

/// <summary>
/// Application service for rental business logic (rent and return cars).
/// </summary>
public class RentalAppService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ICarRepository _carRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICarTypePricingRepository _carTypePricingRepository;
    private readonly CarTypePricingStrategyFactory _pricingStrategyFactory;

    public RentalAppService(
        IRentalRepository rentalRepository,
        ICarRepository carRepository,
        ICustomerRepository customerRepository,
        ICarTypePricingRepository carTypePricingRepository,
        CarTypePricingStrategyFactory pricingStrategyFactory)
    {
        _rentalRepository = rentalRepository;
        _carRepository = carRepository;
        _customerRepository = customerRepository;
        _carTypePricingRepository = carTypePricingRepository;
        _pricingStrategyFactory = pricingStrategyFactory;
    }

    /// <summary>
    /// Creates a new rental (rent a car).
    /// </summary>
    public async Task<(Rental? rental, string? error)> CreateRentalAsync(RentalRequest rental)
    {
        // Validate rental dates
        var today = DateTime.UtcNow.Date;

        if (rental.StartDate.Date < today)
            return (null, "Rental start date cannot be in the past.");

        if (rental.EndDate.Date < rental.StartDate.Date)
            return (null, "Rental end date cannot be before start date.");

        // Validate car availability
        var car = await _carRepository.GetByIdAsync(rental.CarId);

        if (car is null)
            return (null, "Car not found.");

        if (!car.IsAvailable)
            return (null, "Car is not available.");

        // Validate customer existence
        var customer = await _customerRepository.GetByIdAsync(rental.CustomerId);

        if (customer is null)
            return (null, "Customer does not exist.");

        // Check for rental collisions (overlapping dates for the same car)
        bool collision = await _rentalRepository.HasCollisionAsync(rental.CarId, rental.StartDate, rental.EndDate);

        if (collision)
            return (null, "There is already an active or overlapping rental for this car in the selected period.");

        var rentalCreate = new Rental
        {
            Id = Guid.NewGuid(),
            CarId = rental.CarId,
            CustomerId = rental.CustomerId,
            StartDate = rental.StartDate,
            EndDate = rental.EndDate,
        };

        // Assign explicit GUIDs to any AdditionalFees if not set
        if (rentalCreate.AdditionalFees != null)
        {
            foreach (var fee in rentalCreate.AdditionalFees)
            {
                if (fee.Id == Guid.Empty)
                    fee.Id = Guid.NewGuid();
            }
        }

        // Calculate initial price and loyalty points
        var pricing = await _carTypePricingRepository.GetByCarTypeAsync(car.CarType);

        if (pricing is null)
            return (null, "Pricing data not found for car type.");

        // Calculate rental price and loyalty points
        var strategy = _pricingStrategyFactory.GetStrategy(car.CarType);
        rentalCreate.InitialPrice = await strategy.CalculateRentalPriceAsync(rental.StartDate, rental.EndDate, pricing);
        rentalCreate.LoyaltyPointsEarned = strategy.GetLoyaltyPoints(pricing);
        rentalCreate.IsReturned = false;

        // Save rental
        await _rentalRepository.AddAsync(rentalCreate);

        // Update car availability
        car.MarkAsUnavailable();
        await _carRepository.UpdateAsync(car);

        return (rentalCreate, null);
    }

    /// <summary>
    /// Returns a car (close a rental)
    /// </summary>
    public async Task<(Rental? rental, string? error)> ReturnRentalAsync(Guid rentalId, DateTime returnDate)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);

        if (rental is null)
            return (null, "Rental not found.");

        if (rental.IsReturned)
            return (null, "Rental has already been returned.");

        var car = await _carRepository.GetByIdAsync(rental.CarId);

        if (car is null)
            return (null, "Car not found. There is a data issue with this rental");

        rental.AdditionalFees.RemoveAll(f => f.FeeType == Enums.FeeTypeEnum.Late);

        var lateFeeResult = await TryAddLateFeeIfNeeded(rental, returnDate);

        if (lateFeeResult.error is not null)
            return (null, lateFeeResult.error);

        rental.CalculateFinalPrice();
        rental.IsReturned = true;

        await _rentalRepository.UpdateAsync(rental);

        car.MarkAsAvailable();
        await _carRepository.UpdateAsync(car);

        return (rental, null);
    }

    private async Task<(bool added, string? error)> TryAddLateFeeIfNeeded(Rental rental, DateTime returnDate)
    {
        var actualReturnDate = returnDate.Date;
        var scheduledEndDate = rental.EndDate.Date;

        if (actualReturnDate <= scheduledEndDate)
            return (false, null);

        int lateDays = (actualReturnDate - scheduledEndDate).Days;

        if (lateDays <= 0)
            return (false, null);

        var rentalCar = await _carRepository.GetByIdAsync(rental.CarId);
        
        if (rentalCar is null)
            return (false, "Car not found for late fee calculation.");

        var pricing = await _carTypePricingRepository.GetByCarTypeAsync(rentalCar.CarType);

        if (pricing is null)
            return (false, "Pricing data not found for car type.");

        var strategy = _pricingStrategyFactory.GetStrategy(rentalCar.CarType);
        decimal lateFeePerDay;

        try
        {
            lateFeePerDay = await strategy.CalculateLateFeePerDayAsync(pricing);
        }
        catch (Exception ex)
        {
            return (false, $"Late fee configuration error: {ex.Message}");
        }

        var lateFee = new AdditionalFee
        {
            Id = Guid.NewGuid(),
            FeeType = Enums.FeeTypeEnum.Late,
            Amount = lateFeePerDay * lateDays,
            Description = $"Late return: {lateDays} day(s) x {lateFeePerDay}€"
        };
        
        rental.AdditionalFees.Add(lateFee);
        return (true, null);
    }
}
