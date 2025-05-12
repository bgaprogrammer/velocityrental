using CarRentalApi.Core.Entities;
using CarRentalApi.Core.Enums;
using FluentAssertions;
using Moq;

namespace CarRentalApi.Core.Tests.DomainServices
{
    // Tests for rental return scenarios (more in upcoming iterations, enough for MVP)
    public class RentalAppService_ReturnRentalTests : RentalAppServiceTestBase
    {
        [Fact(DisplayName = "Should calculate correct late fee for BMW 7 (Premium) returned 2 days late")]
        public async Task Should_Calculate_Correct_Late_Fee_For_BMW7_Premium_2_Days_Late()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var carId = Guid.NewGuid();
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(5); // original rental for 5 days
            var extraDayLateFee = 360m;
            var basePricePerDay = 300m;
            var loyaltyPoints = 5;
            var expectedRentPrice = 1500m; // 5 days * 300
            var expectedLateFee = 720m; // 2 days late * 360
            var expectedTotalPrice = expectedRentPrice + expectedLateFee;  // 2220

            var rental = new Rental {
                Id = rentalId,
                CarId = carId,
                StartDate = startDate,
                EndDate = endDate,
                AdditionalFees = new List<AdditionalFee>(),
                IsReturned = false,
                InitialPrice = 1500m // 5 days * 300
            };

            var car = new Car {
                Id = carId,
                LicensePlate = "PREM-001",
                Brand = "BMW",
                Model = "7",
                CarType = CarTypeEnum.Premium,
                IsAvailable = false
            };

            var pricing = new CarTypePricing {
                CarType = CarTypeEnum.Premium,
                ExtraDayLateFee = extraDayLateFee,
                BasePricePerDay = basePricePerDay,
                LoyaltyPoints = loyaltyPoints
            };

            RentalRepoMock.Setup(r => r.GetByIdAsync(rentalId)).ReturnsAsync(rental);
            CarRepoMock.Setup(r => r.GetByIdAsync(carId)).ReturnsAsync(car);
            PricingRepoMock.Setup(r => r.GetByCarTypeAsync(CarTypeEnum.Premium)).ReturnsAsync(pricing);
            RentalRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Rental>())).Returns(Task.CompletedTask);
            CarRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Car>())).Returns(Task.CompletedTask);

            // Act
            var returnDate = endDate.AddDays(2); // returned 2 days late
            var (result, error) = await Service.ReturnRentalAsync(rentalId, returnDate);

            // Assert
            result.Should().NotBeNull();
            error.Should().BeNull();
            result!.AdditionalFees.Should().ContainSingle(f => f.FeeType == FeeTypeEnum.Late && f.Amount == expectedLateFee);
            result.FinalPrice.Should().Be(expectedTotalPrice);
        }

        [Fact(DisplayName = "Should calculate correct late fee for Nissan Juke (SUV) returned 1 day late")]
        public async Task Should_Calculate_Correct_Late_Fee_For_NissanJuke_SUV_1_Day_Late()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var carId = Guid.NewGuid();
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(2); // original rental for 2 days
            var basePricePerDay = 150m;
            var loyaltyPoints = 3;
            var extraDayLateFeeFormulaParam = 0.6m;
            var smallBasePricePerDay = 50m;
            var expectedRentPrice = 300m; // 2 days * 150
            var expectedLateFee = 180m; // 1 day late * (150 + 0.6 * 50)
            var expectedTotalPrice = expectedRentPrice + expectedLateFee; // 480

            var rental = new Rental {
                Id = rentalId,
                CarId = carId,
                StartDate = startDate,
                EndDate = endDate,
                AdditionalFees = new List<AdditionalFee>(),
                IsReturned = false,
                InitialPrice = 300m // 2 days * 150
            };

            var car = new Car {
                Id = carId,
                LicensePlate = "SUV-002",
                Brand = "Nissan",
                Model = "Juke",
                CarType = CarTypeEnum.SUV,
                IsAvailable = false
            };

            var pricing = new CarTypePricing {
                CarType = CarTypeEnum.SUV,
                ExtraDayLateFee = null,
                ExtraDayLateFeeFormulaParam = extraDayLateFeeFormulaParam,
                BasePricePerDay = basePricePerDay,
                LoyaltyPoints = loyaltyPoints
            };

            var smallPricing = new CarTypePricing {
                CarType = CarTypeEnum.Small,
                BasePricePerDay = smallBasePricePerDay,
                LoyaltyPoints = 1
            };

            RentalRepoMock.Setup(r => r.GetByIdAsync(rentalId)).ReturnsAsync(rental);
            CarRepoMock.Setup(r => r.GetByIdAsync(carId)).ReturnsAsync(car);
            PricingRepoMock.Setup(r => r.GetByCarTypeAsync(CarTypeEnum.SUV)).ReturnsAsync(pricing);
            PricingRepoMock.Setup(r => r.GetByCarTypeAsync(CarTypeEnum.Small)).ReturnsAsync(smallPricing);
            RentalRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Rental>())).Returns(Task.CompletedTask);
            CarRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Car>())).Returns(Task.CompletedTask);

            // Act
            var returnDate = endDate.AddDays(1); // returned 1 day late
            var (result, error) = await Service.ReturnRentalAsync(rentalId, returnDate);

            // Assert
            result.Should().NotBeNull();
            error.Should().BeNull();
            result!.AdditionalFees.Should().ContainSingle(f => f.FeeType == FeeTypeEnum.Late && f.Amount == expectedLateFee);
            result.FinalPrice.Should().Be(expectedTotalPrice);
        }
    }
}