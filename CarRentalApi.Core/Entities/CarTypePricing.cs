using CarRentalApi.Core.Enums;

namespace CarRentalApi.Core.Entities;

/// <summary>
/// Master data for car type pricing and late fee rules.
/// </summary>
public class CarTypePricing
{
    public Guid Id { get; set; }

    public CarTypeEnum CarType { get; set; }

    public decimal BasePricePerDay { get; set; }

    public decimal? ExtraDayLateFee { get; set; } // used for fixed amount like Premium

    public decimal? DiscountAfter7Days { get; set; } // 0.8m for 80%

    public decimal? DiscountAfter30Days { get; set; } // 0.5m for 50%

    public decimal? ExtraDayLateFeeFormulaParam { get; set; } // For complex rules (% of another type)

    public int LoyaltyPoints { get; set; } // Loyalty points per rental for this car type
    
    public string? Notes { get; set; }
}
