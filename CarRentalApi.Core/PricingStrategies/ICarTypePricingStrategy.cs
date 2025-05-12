using CarRentalApi.Core.Entities;

namespace CarRentalApi.Core.PricingStrategies;

/// <summary>
/// Strategy interface for car type pricing and late fee logic.
/// </summary>
public interface ICarTypePricingStrategy
{
    Task<decimal> CalculateRentalPriceAsync(DateTime start, DateTime end, CarTypePricing pricing);
    
    Task<decimal> CalculateLateFeePerDayAsync(CarTypePricing pricing);

    int GetLoyaltyPoints(CarTypePricing pricing);

    public static int CalculateRentalDays(DateTime start, DateTime end)
    {
        return (int)(end.Date - start.Date).TotalDays + 1;
    }
}
