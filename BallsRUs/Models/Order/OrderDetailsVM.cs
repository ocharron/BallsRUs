using BallsRUs.Entities;
using BallsRUs.Models.Checkout;
using System.ComponentModel.DataAnnotations;

namespace BallsRUs.Models.Order
{
    public class OrderDetailsVM
    {
        public Guid Id { get; set; }
        public string? Number { get; set; }
        public OrderStatus Status { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public int ProductQuantity { get; set; }
        public string? ProductsCost { get; set; }
        public string? ShippingCost { get; set; }
        public string? SubTotal { get; set; }
        public string? Taxes { get; set; }
        public string? Total { get; set; }
        public string? AddressStreet { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressStateProvince { get; set; }
        public string? AddressCountry { get; set; }
        public string? AddressPostalCode { get; set; }
        public bool IsPayed { get; set; } = false;
        public string? CreationDate { get; set; }
        public string? ConfirmationDate { get; set; }
        public string? ModificationDate { get; set; }
        public List<OrderDetailsItemVM>? OrderItems { get; set; }
    }
}