namespace BallsRUs.Models.Checkout
{
    public class CheckoutReceiptVM
    {
        public Guid? Id { get; set; }
        public string? OrderFullName { get; set; }
        public string? OrderNumber { get; set; }
        public string? OrderEmailAddress { get; set; }
        public string? OrderPhoneNumber { get; set; }
        public string? OrderTotal { get; set; }
        public string? PaymentFullName { get; set; }
        public string? PaymentLast4 { get; set; }
        public string? PaymentDate { get; set; }
        public string? AddressStreet { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressStateProvince { get; set; }
        public string? AddressCountry { get; set; }
        public string? AddressPostalCode { get; set; }
    }
}
