using CarRentalApi.Core.Entities;
using CarRentalApi.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using CarRentalApi.Infrastructure.Persistence;

namespace CarRentalApi.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of IRentalRepository.
/// </summary>
public class RentalRepository : IRentalRepository
{
    private readonly CarRentalDbContext _context;

    public RentalRepository(CarRentalDbContext context) => _context = context;

    public async Task<Rental?> GetByIdAsync(Guid id) => await _context.Rentals.Include(r => r.AdditionalFees).FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Rental>> GetAllAsync() => await _context.Rentals.Include(r => r.AdditionalFees).ToListAsync();

    public async Task AddAsync(Rental rental)
    {
        _context.Rentals.Add(rental);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Rental rental)
    {
        _context.Rentals.Update(rental);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var rental = await _context.Rentals.FindAsync(id);

        if (rental is not null)
        {
            _context.Rentals.Remove(rental);

            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Rental>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Rentals.Include(r => r.AdditionalFees).Where(r => r.CustomerId == customerId).ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetByCarIdAsync(Guid carId)
    {
        return await _context.Rentals.Include(r => r.AdditionalFees).Where(r => r.CarId == carId).ToListAsync();
    }

    public async Task<bool> HasCollisionAsync(Guid carId, DateTime startDate, DateTime endDate)
    {
        return await _context.Rentals.AnyAsync(r =>
            r.CarId == carId && !r.IsReturned &&
            startDate.Date <= r.EndDate.Date && endDate.Date >= r.StartDate.Date
        );
    }
}
