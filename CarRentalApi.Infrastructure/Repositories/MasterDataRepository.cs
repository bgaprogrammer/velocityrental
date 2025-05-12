using CarRentalApi.Core.Repositories;
using CarRentalApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApi.Infrastructure.Repositories;

public class MasterDataRepository : IMasterDataRepository
{
    private readonly CarRentalDbContext _db;
    private int recordsCount = 0;


    public MasterDataRepository(CarRentalDbContext db)
    {
        _db = db;
        recordsCount = MasterDataHardcoded.GetCarTypePricing().Count +
                       MasterDataHardcoded.GetCars().Count +
                       MasterDataHardcoded.GetCustomers().Count;
    }

    public async Task InitializeAsync()
    {
        // Clean first to avoid duplicates
        await CleanAsync();

        _db.CarTypePricings.AddRange(MasterDataHardcoded.GetCarTypePricing());
        await _db.SaveChangesAsync();

        _db.Cars.AddRange(MasterDataHardcoded.GetCars());
        await _db.SaveChangesAsync();

        _db.Customers.AddRange(MasterDataHardcoded.GetCustomers());
        await _db.SaveChangesAsync();
    }

    public async Task CleanAsync()
    {
        _db.Rentals.RemoveRange(_db.Rentals);
        _db.Cars.RemoveRange(_db.Cars);
        _db.Customers.RemoveRange(_db.Customers);
        _db.CarTypePricings.RemoveRange(_db.CarTypePricings);

        await _db.SaveChangesAsync();
    }

    public async Task<bool> IsInitializedAsync()
    {
        var carCount = await _db.Cars.CountAsync();
        var pricingCount = await _db.CarTypePricings.CountAsync();
        var customerCount = await _db.Customers.CountAsync();

        return carCount + pricingCount + customerCount == recordsCount;
    }
}
