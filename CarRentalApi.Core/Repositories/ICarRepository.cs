using CarRentalApi.Core.Entities;

namespace CarRentalApi.Core.Repositories;

/// <summary>
/// Repository abstraction for Car entity.
/// </summary>
public interface ICarRepository
{
    Task<Car?> GetByIdAsync(Guid id);

    Task UpdateAsync(Car car);
}
