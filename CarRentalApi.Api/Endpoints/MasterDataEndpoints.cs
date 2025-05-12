using CarRentalApi.Core.Repositories;
using CarRentalApi.Core.Constants;

namespace CarRentalApi.Api.Endpoints;

public static class MasterDataEndpoints
{
    public static IEndpointRouteBuilder MapMasterDataEndpoints(this IEndpointRouteBuilder app)
    {
        // Initializes master data (cars, pricing, customers).
        app.MapPost("/api/MasterData/initialize", async (IMasterDataRepository repo) =>
        {
            await repo.InitializeAsync();

            return Results.Ok("Master data initialized.");
        })
        .WithName("InitializeMasterData")
        .WithSummary("Initializes master data (cars, pricing, customers).")
        .WithTags(ApiTags.MasterData);

        // Cleans all data from the database (cars, pricing, customers, rentals).
        app.MapPost("/api/MasterData/clean", async (IMasterDataRepository repo) =>
        {
            await repo.CleanAsync();

            return Results.Ok("All data deleted.");
        })
        .WithName("CleanMasterData")
        .WithSummary("Cleans all data from the database (cars, pricing, customers, rentals).")
        .WithTags(ApiTags.MasterData);

        // Checks if master data is initialized.
        app.MapGet("/api/MasterData/status", async (IMasterDataRepository repo) =>
        {
            var isInitialized = await repo.IsInitializedAsync();
            
            if (isInitialized)
                return Results.Ok("The database is already initialized.");
            else
                return Results.Ok("There is no data, use initialize method first to avoid errors in the testing.");
        })
        .WithName("MasterDataStatus")
        .WithSummary("Checks if master data is initialized.")
        .WithTags(ApiTags.MasterData);

        return app;
    }
}
