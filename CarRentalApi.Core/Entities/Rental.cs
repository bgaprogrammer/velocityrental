namespace CarRentalApi.Core.Entities;

/// <summary>
/// Represents a rental transaction.
/// </summary>
public class Rental
{
    public Guid Id { get; set; }

    public Guid CarId { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    /// <summary>
    /// Price agreed at the start of the rental (without any additional fees).
    /// </summary>
    public decimal InitialPrice { get; set; }

    /// <summary>
    /// Final price paid by the customer, including any additional fees (e.g., late return).
    /// </summary>
    public decimal FinalPrice { get; set; }

    /// <summary>
    /// List of additional fees applied to this rental.
    /// </summary>
    public List<AdditionalFee> AdditionalFees { get; set; } = [];

    // TODO: Create another table for saving loyalty points history (future iteration)
    public int LoyaltyPointsEarned { get; set; }

    public bool IsReturned { get; set; }

    public void CalculateFinalPrice()
    {
        FinalPrice = InitialPrice;
        if (AdditionalFees is not null)
        {
            FinalPrice += AdditionalFees.Sum(fee => fee.Amount);
        }
    }
}
