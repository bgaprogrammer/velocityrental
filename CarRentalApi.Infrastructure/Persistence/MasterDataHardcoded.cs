using CarRentalApi.Core.Entities;
using CarRentalApi.Core.Enums;

namespace CarRentalApi.Infrastructure.Persistence;

/// <summary>
/// Provides hardcoded master data for initialization.
/// </summary>
public static class MasterDataHardcoded
{
    public static List<CarTypePricing> GetCarTypePricing() =>
    [
        new CarTypePricing
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
            CarType = CarTypeEnum.Premium,
            BasePricePerDay = 300m,
            ExtraDayLateFee = 360m,
            DiscountAfter7Days = null,
            DiscountAfter30Days = null,
            ExtraDayLateFeeFormulaParam = null,
            LoyaltyPoints = 5,
            Notes = "Premium: 300€/day, late: 360€/day, loyalty: 5"
        },
        new CarTypePricing
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
            CarType = CarTypeEnum.SUV,
            BasePricePerDay = 150m,
            ExtraDayLateFee = null,
            DiscountAfter7Days = 0.8m,
            DiscountAfter30Days = 0.5m,
            ExtraDayLateFeeFormulaParam = 0.6m,
            LoyaltyPoints = 3,
            Notes = "SUV: 150€/day, 80% after 7d, 50% after 30d, late: 150 + 60% of small/day, loyalty: 3"
        },
        new CarTypePricing
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
            CarType = CarTypeEnum.Small,
            BasePricePerDay = 50m,
            ExtraDayLateFee = null,
            DiscountAfter7Days = 0.6m,
            DiscountAfter30Days = null,
            ExtraDayLateFeeFormulaParam = 0.3m,
            LoyaltyPoints = 1,
            Notes = "Small: 50€/day, 60% after 7d, late: 50 + 30% of small/day, loyalty: 1"
        }
    ];

    public static List<Car> GetCars() =>
    [
        new Car
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            LicensePlate = "PREM-001",
            Brand = "BMW",
            Model = "Serie 7",
            CarType = CarTypeEnum.Premium,
            IsAvailable = true
        },
        new Car
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            LicensePlate = "SUV-001",
            Brand = "Toyota",
            Model = "Sorento",
            CarType = CarTypeEnum.SUV,
            IsAvailable = true
        },
        new Car
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            LicensePlate = "SUV-002",
            Brand = "Nissan",
            Model = "Juke",
            CarType = CarTypeEnum.SUV,
            IsAvailable = true
        },
        new Car
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            LicensePlate = "SMALL-001",
            Brand = "Seat",
            Model = "Ibiza",
            CarType = CarTypeEnum.Small,
            IsAvailable = true
        }
    ];

    public static List<Customer> GetCustomers() =>
    [
        new Customer
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Name = "Jorge Ramirez",
            Email = "jr@jorgeramirez.com",
            LoyaltyPoints = 0
        },
        new Customer
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Name = "Peter Parker",
            Email = "pepa@mcu.com",
            LoyaltyPoints = 0
        }
    ];
}
