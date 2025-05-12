
using CarRentalApi.Core.Entities;
using CarRentalApi.Core.Enums;
using CarRentalApi.Core.Constants;

namespace CarRentalApi.Core.PricingStrategies;

public class SmallCarPricingStrategy : ICarTypePricingStrategy
{
    public SmallCarPricingStrategy()
    {
    }

    public Task<decimal> CalculateRentalPriceAsync(DateTime start, DateTime end, CarTypePricing pricing)
    {
        int days = ICarTypePricingStrategy.CalculateRentalDays(start, end);
        decimal total = 0;

        var discountAfter7 = pricing.DiscountAfter7Days ?? throw new InvalidOperationException(CarTypePricingExceptionMessages.DiscountAfter7DaysNotConfigured(CarTypeEnum.Small));

        if (days > 7)
        {
            total += pricing.BasePricePerDay * 7; // first 7 days
            total += pricing.BasePricePerDay * discountAfter7 * (days - 7); // days 8+
        }
        else
        {
            total += pricing.BasePricePerDay * days;
        }

        return Task.FromResult(total);
    }

    public Task<decimal> CalculateLateFeePerDayAsync(CarTypePricing pricing)
    {
        if (pricing.ExtraDayLateFeeFormulaParam == null)
            throw new InvalidOperationException(CarTypePricingExceptionMessages.ExtraDayLateFeeFormulaParamNotConfigured(CarTypeEnum.Small));

        var percent = pricing.ExtraDayLateFeeFormulaParam.Value;

        return Task.FromResult(pricing.BasePricePerDay + (percent * pricing.BasePricePerDay));
    }

    public int GetLoyaltyPoints(CarTypePricing pricing)
    {
        return pricing.LoyaltyPoints;
    }
}
