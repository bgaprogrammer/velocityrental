using CarRentalApi.Core.Entities;
using CarRentalApi.Core.Enums;

namespace CarRentalApi.Core.Repositories;

/// <summary>
/// Repository abstraction for CarTypePricing entity.
/// </summary>
public interface ICarTypePricingRepository
{
    Task<CarTypePricing?> GetByCarTypeAsync(CarTypeEnum carType);
}
