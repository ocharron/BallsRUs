using Stripe;
using System.ComponentModel.DataAnnotations.Schema;

namespace BallsRUs.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? StateProvince { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Digits { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? StripePaymentId { get; set; }
    }
}
