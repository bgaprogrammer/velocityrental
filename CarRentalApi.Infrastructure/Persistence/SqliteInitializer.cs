using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalApi.Infrastructure.Persistence;

/// <summary>
/// Ensures the SQLite database is created and migrated at startup.
/// </summary>
public static class SqliteInitializer
{
    public static void EnsureDatabaseCreated(IServiceProvider serviceProvider)
    {
        var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
        db.Database.Migrate();
    }
}
