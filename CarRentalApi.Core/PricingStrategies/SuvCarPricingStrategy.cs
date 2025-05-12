using CarRentalApi.Core.Entities;
using CarRentalApi.Core.Enums;
using CarRentalApi.Core.Repositories;
using CarRentalApi.Core.Constants;

namespace CarRentalApi.Core.PricingStrategies;

public class SuvCarPricingStrategy : ICarTypePricingStrategy
{
    private readonly ICarTypePricingRepository _carTypePricingRepository;

    public SuvCarPricingStrategy(ICarTypePricingRepository carTypePricingRepository)
    {
        _carTypePricingRepository = carTypePricingRepository;
    }

    public Task<decimal> CalculateRentalPriceAsync(DateTime start, DateTime end, CarTypePricing pricing)
    {
        int days = ICarTypePricingStrategy.CalculateRentalDays(start, end);
        decimal total = 0;

        var discountAfter7 = pricing.DiscountAfter7Days ?? throw new InvalidOperationException(CarTypePricingExceptionMessages.DiscountAfter7DaysNotConfigured(CarTypeEnum.SUV));
        var discountAfter30 = pricing.DiscountAfter30Days ?? throw new InvalidOperationException(CarTypePricingExceptionMessages.DiscountAfter30DaysNotConfigured(CarTypeEnum.SUV));

        if (days > 30)
        {
            total += pricing.BasePricePerDay * 7; // first 7 days
            total += pricing.BasePricePerDay * discountAfter7 * 23; // days 8-30
            total += pricing.BasePricePerDay * discountAfter30 * (days - 30); // days 31+
        }
        else if (days > 7)
        {
            total += pricing.BasePricePerDay * 7; // first 7 days
            total += pricing.BasePricePerDay * discountAfter7 * (days - 7); // days 8+
        }
        else
        {
            total += pricing.BasePricePerDay * days; // 7 days
        }

        return Task.FromResult(total);
    }

    public async Task<decimal> CalculateLateFeePerDayAsync(CarTypePricing pricing)
    {        
        if (pricing.ExtraDayLateFeeFormulaParam == null)
            throw new InvalidOperationException(CarTypePricingExceptionMessages.ExtraDayLateFeeFormulaParamNotConfigured(CarTypeEnum.SUV));

        var percent = pricing.ExtraDayLateFeeFormulaParam.Value;
        
        var smallPricing = await _carTypePricingRepository.GetByCarTypeAsync(CarTypeEnum.Small) ?? throw new InvalidOperationException(CarTypePricingExceptionMessages.CarPricingNotFound(CarTypeEnum.Small));
        
        return pricing.BasePricePerDay + (percent * smallPricing.BasePricePerDay);
    }

    public int GetLoyaltyPoints(CarTypePricing pricing)
    {
        return pricing.LoyaltyPoints;
    }
}
