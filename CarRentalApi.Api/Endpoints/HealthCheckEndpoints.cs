namespace CarRentalApi.Api.Endpoints;

public static class HealthCheckEndpoints
{
    public static IEndpointRouteBuilder MapHealthCheckEndpoints(this IEndpointRouteBuilder app)
    {
        // Health check endpoint
        app.MapGet("/healthz", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
            .WithDescription("Health check endpoint for monitoring the API status.")
            .ExcludeFromDescription();

        return app;
    }
}
