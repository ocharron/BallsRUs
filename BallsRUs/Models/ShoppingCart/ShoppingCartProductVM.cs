using System.Globalization;

namespace BallsRUs.Models.ShoppingCart
{
    public class ShoppingCartProductVM
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public string? Name { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public string? ItemTotalRetailPrice { get => string.Format(new CultureInfo("fr-CA"), "{0:C}", RetailPrice * Quantity); }
        public string? ItemTotalDiscountedPrice { get => DiscountedPrice is not null ? string.Format(new CultureInfo("fr-CA"), "{0:C}", DiscountedPrice * Quantity) : null; }
        public string? ImagePath { get; set; }
        public int? Quantity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
