using CarRentalApi.Core.Enums;

namespace CarRentalApi.Core.Entities;

/// <summary>
/// Represents an additional fee or charge applied to a rental.
/// </summary>
public class AdditionalFee
{
    public Guid Id { get; set; }

    public FeeTypeEnum FeeType { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }
}
