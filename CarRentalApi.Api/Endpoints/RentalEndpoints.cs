using CarRentalApi.Core.Entities;
using CarRentalApi.Core.Repositories;
using CarRentalApi.Core.DomainServices;
using CarRentalApi.Core.Constants;

namespace CarRentalApi.Api.Endpoints;

public static class RentalEndpoints
{
    public static IEndpointRouteBuilder MapRentalEndpoints(this IEndpointRouteBuilder app)
    {
        // Create a new rental
        app.MapPost("/api/Rental", async (Rental rental, RentalAppService rentalAppService) =>
        {
            var (createdRental, error) = await rentalAppService.CreateRentalAsync(rental);

            if (error != null)
                return Results.BadRequest(error);

            return Results.Created($"/api/Rental/{createdRental!.Id}", createdRental);
        })
        .WithName("CreateRental")
        .WithSummary("Creates a new rental (rent a car). Returns 201 Created on success, 400 Bad Request on error.")
        .Produces<Rental>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithTags(ApiTags.RentingOperations);

        // Return a car (close a rental)
        app.MapPost("/api/Rental/{id:guid}/return", async (Guid id, DateTime? returnDate, RentalAppService rentalAppService) =>
        {
            var effectiveReturnDate = returnDate ?? DateTime.UtcNow;

            var (returnedRental, error) = await rentalAppService.ReturnRentalAsync(id, effectiveReturnDate);

            if (error != null)
                return Results.NotFound(error);

            return Results.Ok(returnedRental);
        })
        .WithName("ReturnRental")
        .WithSummary("Returns a car (close a rental). Returns 200 OK on success, 404 Not Found if rental does not exist.")
        .Produces<Rental>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags(ApiTags.RentingOperations);

        // Get all rentals (hidden from Swagger)
        app.MapGet("/api/Rental", async (IRentalRepository rentalRepository) =>
        {
            var rentals = await rentalRepository.GetAllAsync();

            return Results.Ok(rentals);
        })
        .WithName("GetAllRentals")
        .WithSummary("Gets all rentals in the system. (just for testing purposes)")
        .ExcludeFromDescription() // Exclude from Swagger documentation
        .Produces<IEnumerable<Rental>>(StatusCodes.Status200OK)
        .WithTags(ApiTags.RentingOperations);

        // Get rental by ID (hidden from Swagger)
        app.MapGet("/api/Rental/{id:guid}", async (Guid id, IRentalRepository rentalRepository) =>
        {
            var rental = await rentalRepository.GetByIdAsync(id);

            if (rental is null)
                return Results.NotFound();
                
            return Results.Ok(rental);
        })
        .WithName("GetRentalById")
        .WithSummary("Gets a rental by ID. (just for testing purposes)")
        .ExcludeFromDescription() // Exclude from Swagger documentation
        .Produces<Rental>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags(ApiTags.RentingOperations);

        return app;
    }
}
