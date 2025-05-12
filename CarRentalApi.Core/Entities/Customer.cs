namespace CarRentalApi.Core.Entities;

/// <summary>
/// Represents a customer in the rental system.
/// </summary>
public class Customer
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public int LoyaltyPoints { get; set; }
}
