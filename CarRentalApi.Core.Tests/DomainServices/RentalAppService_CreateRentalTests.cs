using CarRentalApi.Core.Entities;
using CarRentalApi.Core.Enums;
using FluentAssertions;
using Moq;

namespace CarRentalApi.Core.Tests.DomainServices
{
    // Tests for rental creation scenarios (enough for MVP)
    public class RentalAppService_CreateRentalTests : RentalAppServiceTestBase
    {
        [Theory(DisplayName = "Should calculate correct price for various car types and durations (CreateRental)")]
        [InlineData("BMW 7", CarTypeEnum.Premium, 10, 3000)]
        [InlineData("Kia Sorento", CarTypeEnum.SUV, 9, 1290)]
        [InlineData("Nissan Juke", CarTypeEnum.SUV, 2, 300)]
        [InlineData("Seat Ibiza", CarTypeEnum.Small, 10, 440)]
        public async Task Should_Calculate_Correct_Price_For_CarType_And_Duration(string model, CarTypeEnum carType, int days, decimal expectedPrice)
        {
            // Arrange
            var carId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var startDate = DateTime.UtcNow.Date;
            var endDate = startDate.AddDays(days - 1);
            var rental = new Rental { CarId = carId, CustomerId = customerId, StartDate = startDate, EndDate = endDate };

            var car = new Car {
                Id = carId,
                IsAvailable = true,
                LicensePlate = "TEST123",
                Brand = model.Contains("BMW") ? "BMW" : model.Contains("Kia") ? "Kia" : model.Contains("Nissan") ? "Nissan" : "Seat",
                Model = model,
                CarType = carType
            };

            var customer = new Customer {
                Id = customerId,
                Name = "Test User",
                Email = "test@example.com",
                LoyaltyPoints = 0
            };

            // Pricing rules based on MasterDataHardcoded
            CarTypePricing pricing = carType switch
            {
                CarTypeEnum.Premium => new CarTypePricing {
                    CarType = CarTypeEnum.Premium,
                    BasePricePerDay = 300m,
                    ExtraDayLateFee = 360m,
                    DiscountAfter7Days = null,
                    DiscountAfter30Days = null,
                    ExtraDayLateFeeFormulaParam = null,
                    LoyaltyPoints = 5
                },
                CarTypeEnum.SUV => new CarTypePricing {
                    CarType = CarTypeEnum.SUV,
                    BasePricePerDay = 150m,
                    ExtraDayLateFee = null,
                    DiscountAfter7Days = 0.8m,
                    DiscountAfter30Days = 0.5m,
                    ExtraDayLateFeeFormulaParam = 0.6m,
                    LoyaltyPoints = 3
                },
                CarTypeEnum.Small => new CarTypePricing {
                    CarType = CarTypeEnum.Small,
                    BasePricePerDay = 50m,
                    ExtraDayLateFee = null,
                    DiscountAfter7Days = 0.6m,
                    DiscountAfter30Days = null,
                    ExtraDayLateFeeFormulaParam = 0.3m,
                    LoyaltyPoints = 1
                },
                _ => throw new NotImplementedException()
            };

            CarRepoMock.Setup(r => r.GetByIdAsync(carId)).ReturnsAsync(car);
            CustomerRepoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);
            RentalRepoMock.Setup(r => r.HasCollisionAsync(carId, startDate, endDate)).ReturnsAsync(false);
            PricingRepoMock.Setup(r => r.GetByCarTypeAsync(carType)).ReturnsAsync(pricing);
            RentalRepoMock.Setup(r => r.AddAsync(It.IsAny<Rental>())).Returns(Task.CompletedTask);
            CarRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Car>())).Returns(Task.CompletedTask);

            // Act
            var (result, error) = await Service.CreateRentalAsync(rental);

            // Assert
            result.Should().NotBeNull();
            error.Should().BeNull();
            result.InitialPrice.Should().Be(expectedPrice);
        }
    }
}
