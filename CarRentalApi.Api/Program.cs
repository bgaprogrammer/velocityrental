using Microsoft.EntityFrameworkCore;
using CarRentalApi.Infrastructure.Persistence;
using CarRentalApi.Infrastructure.Repositories;
using CarRentalApi.Core.Repositories;
using CarRentalApi.Core.DomainServices;
using CarRentalApi.Core.Enums;
using CarRentalApi.Core.PricingStrategies;

using CarRentalApi.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to use the PORT environment variable
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Car Rental API",
        Version = "v1",
        Description = "API developed by Jorge Ramírez © 2025. All rights reserved.",
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "All rights reserved. No use, distribution or modification without explicit written consent from Jorge Ramírez.",
            Url = new Uri("https://github.com/bgaprogrammer")
        }
    });
});
builder.Services.AddControllers();

// Register DbContext with SQLite
builder.Services.AddDbContext<CarRentalDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<ICarTypePricingRepository, CarTypePricingRepository>();
builder.Services.AddScoped<IMasterDataRepository, MasterDataRepository>();

// Register pricing strategies
builder.Services.AddKeyedScoped<ICarTypePricingStrategy, PremiumCarPricingStrategy>(CarTypeEnum.Premium);
builder.Services.AddKeyedScoped<ICarTypePricingStrategy, SuvCarPricingStrategy>(CarTypeEnum.SUV);
builder.Services.AddKeyedScoped<ICarTypePricingStrategy, SmallCarPricingStrategy>(CarTypeEnum.Small);

builder.Services.AddScoped<IReadOnlyDictionary<CarTypeEnum, ICarTypePricingStrategy>>(sp => new Dictionary<CarTypeEnum, ICarTypePricingStrategy>
{
    [CarTypeEnum.Premium] = sp.GetRequiredKeyedService<ICarTypePricingStrategy>(CarTypeEnum.Premium),
    [CarTypeEnum.SUV] = sp.GetRequiredKeyedService<ICarTypePricingStrategy>(CarTypeEnum.SUV),
    [CarTypeEnum.Small] = sp.GetRequiredKeyedService<ICarTypePricingStrategy>(CarTypeEnum.Small)
});

// Register pricing strategies factory
builder.Services.AddScoped<CarTypePricingStrategyFactory>();

// Register domain services
builder.Services.AddScoped<RentalAppService>();

var app = builder.Build();

SqliteInitializer.EnsureDatabaseCreated(app.Services);

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); // Not required for local development

// Health check endpoint
app.MapHealthCheckEndpoints();

// MasterData endpoints
app.MapMasterDataEndpoints();

// Rental endpoints
app.MapRentalEndpoints();

app.MapControllers();

app.Run();
