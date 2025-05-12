using CarRentalApi.Core.Entities;

namespace CarRentalApi.Core.Repositories;

/// <summary>
/// Repository abstraction for Rental entity.
/// </summary>
public interface IRentalRepository
{
    Task<Rental?> GetByIdAsync(Guid id);

    Task<IEnumerable<Rental>> GetAllAsync();

    Task AddAsync(Rental rental);

    Task UpdateAsync(Rental rental);

    Task<bool> HasCollisionAsync(Guid carId, DateTime startDate, DateTime endDate);
}
