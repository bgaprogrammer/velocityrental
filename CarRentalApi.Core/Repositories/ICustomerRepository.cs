using CarRentalApi.Core.Entities;

namespace CarRentalApi.Core.Repositories;

/// <summary>
/// Repository abstraction for Customer entity.
/// </summary>
public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
}
