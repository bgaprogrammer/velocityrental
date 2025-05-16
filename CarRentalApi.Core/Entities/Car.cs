using CarRentalApi.Core.Enums;

namespace CarRentalApi.Core.Entities;

/// <summary>
/// Represents a car in the rental system.
/// </summary>
public class Car
{
    public Guid Id { get; set; }

    public required string LicensePlate { get; set; }

    public required string Brand { get; set; }

    public required string Model { get; set; }

    public CarTypeEnum CarType { get; set; }

    public bool IsAvailable { get; set; } = true;

    public void MarkAsAvailable()
    {
        IsAvailable = true;
    }

    public void MarkAsUnavailable()
    {
        IsAvailable = false;
    }
}
