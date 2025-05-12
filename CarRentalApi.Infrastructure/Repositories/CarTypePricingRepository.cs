using CarRentalApi.Core.Entities;
using CarRentalApi.Core.Enums;
using CarRentalApi.Core.Repositories;
using CarRentalApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApi.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of ICarTypePricingRepository.
/// </summary>
public class CarTypePricingRepository : ICarTypePricingRepository
{
    private readonly CarRentalDbContext _context;

    public CarTypePricingRepository(CarRentalDbContext context) => _context = context;

    public async Task<CarTypePricing?> GetByCarTypeAsync(CarTypeEnum carType) => await _context.CarTypePricings.FirstOrDefaultAsync(p => p.CarType == carType);
}
