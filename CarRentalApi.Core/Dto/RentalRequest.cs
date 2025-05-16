namespace CarRentalApi.Core.Dto
{
    public class RentalRequest
    {
        public Guid CarId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
