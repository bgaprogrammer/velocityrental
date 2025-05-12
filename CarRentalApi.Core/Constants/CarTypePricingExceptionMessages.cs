using CarRentalApi.Core.Enums;
using CarRentalApi.Core.Entities;

namespace CarRentalApi.Core.Constants;

public static class CarTypePricingExceptionMessages
{
    public const string DiscountAfter7DaysField = nameof(CarTypePricing.DiscountAfter7Days);
    public const string DiscountAfter30DaysField = nameof(CarTypePricing.DiscountAfter30Days);
    public const string ExtraDayLateFeeFormulaParamField = nameof(CarTypePricing.ExtraDayLateFeeFormulaParam);
    public const string ExtraDayLateFeeField = nameof(CarTypePricing.ExtraDayLateFee);
    public static string DiscountAfter7DaysNotConfigured(CarTypeEnum carType) => $"{DiscountAfter7DaysField} is not configured for {carType} car type.";
    public static string DiscountAfter30DaysNotConfigured(CarTypeEnum carType) => $"{DiscountAfter30DaysField} is not configured for {carType} car type.";
    public static string ExtraDayLateFeeFormulaParamNotConfigured(CarTypeEnum carType) => $"{ExtraDayLateFeeFormulaParamField} is not configured for {carType} car type.";
    public static string ExtraDayLateFeeNotConfigured(CarTypeEnum carType) => $"{ExtraDayLateFeeField} is not configured for {carType} car type.";
    public static string CarPricingNotFound(CarTypeEnum carType) => $"{carType} car pricing not found";
}
