namespace BallsRUs.Models.Order
{
    public class OrderDetailsItemVM
    {
        public Guid? Id { get; set; }
        public int? Quantity { get; set; }
        public string? TotalCost { get; set; }
        public string? ProductName { get; set; }
    }
}
