var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
// TODO: Register infrastructure and core services

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Not required for local development

// Simple test endpoint
app.MapGet("/test", () => Results.Ok("running"))
    .WithName("TestEndpoint");

app.MapControllers();

app.Run();
