using CarRentalApi.Core.Entities;
using CarRentalApi.Core.Repositories;
using CarRentalApi.Infrastructure.Persistence;

namespace CarRentalApi.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of ICarRepository.
/// </summary>
public class CarRepository : ICarRepository
{
    private readonly CarRentalDbContext _context;

    public CarRepository(CarRentalDbContext context) => _context = context;

    public async Task<Car?> GetByIdAsync(Guid id) => await _context.Cars.FindAsync(id);

    public async Task UpdateAsync(Car car)
    {
        _context.Cars.Update(car);
        await _context.SaveChangesAsync();
    }
}
