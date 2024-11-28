using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace BallsRUs.Entities
{
    public enum OrderStatus { Opened, Confirmed, Payed, Refunded, Canceled }

    public class Order
    {
        public Guid Id { get; set; }
        public string? Number { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Opened;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public int ProductQuantity { get; set; }
        public decimal? ProductsCost { get; set; }
        public decimal? ShippingCost { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Taxes { get; set; }
        public decimal? Total { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        // Foreign keys
        public Guid? UserId { get; set; }
        public Guid AddressId { get; set; }
        public Guid? PaymentId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        [ForeignKey(nameof(AddressId))]
        public virtual Address? Address { get; set; }

        [ForeignKey(nameof(PaymentId))]
        public virtual Payment? Payment { get; set; }
    }
}
