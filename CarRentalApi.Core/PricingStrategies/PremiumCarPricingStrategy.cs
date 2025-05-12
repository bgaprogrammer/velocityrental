
using CarRentalApi.Core.Entities;
using CarRentalApi.Core.Enums;
using CarRentalApi.Core.Constants;

namespace CarRentalApi.Core.PricingStrategies;

public class PremiumCarPricingStrategy : ICarTypePricingStrategy
{
    public PremiumCarPricingStrategy()
    {
    }

    public Task<decimal> CalculateRentalPriceAsync(DateTime start, DateTime end, CarTypePricing pricing)
    {
        int days = ICarTypePricingStrategy.CalculateRentalDays(start, end);
        
        return Task.FromResult(pricing.BasePricePerDay * days);
    }

    public Task<decimal> CalculateLateFeePerDayAsync(CarTypePricing pricing)
    {
        if (pricing.ExtraDayLateFee == null)
            throw new InvalidOperationException(CarTypePricingExceptionMessages.ExtraDayLateFeeNotConfigured(CarTypeEnum.Premium));

        return Task.FromResult(pricing.ExtraDayLateFee.Value);
    }

    public int GetLoyaltyPoints(CarTypePricing pricing)
    {
        return pricing.LoyaltyPoints;
    }
}
