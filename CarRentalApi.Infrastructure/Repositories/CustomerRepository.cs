using CarRentalApi.Core.Entities;
using CarRentalApi.Core.Repositories;
using CarRentalApi.Infrastructure.Persistence;

namespace CarRentalApi.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of ICustomerRepository.
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly CarRentalDbContext _context;

    public CustomerRepository(CarRentalDbContext context) => _context = context;

    public async Task<Customer?> GetByIdAsync(Guid id) => await _context.Customers.FindAsync(id);
}
