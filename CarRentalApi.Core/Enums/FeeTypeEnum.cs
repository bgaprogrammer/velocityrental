namespace CarRentalApi.Core.Enums;

/// <summary>
/// Enumeration of possible additional fee types.
/// </summary>
public enum FeeTypeEnum
{
    None = 0,

    /// <summary>
    /// Fee charged for returning the car after the agreed end date.
    /// </summary>
    Late = 1
}
