using BallsRUs.Utilities;
using System.Globalization;

namespace BallsRUs.Models.ShoppingCart
{
    public class ShoppingCartListVM
    {
        public int? Quantity { get; set; }
        public decimal? ProductsCost { get; set; }
        public decimal? ShippingCost { get; set; }
        public decimal? SubTotal { get => ProductsCost + ShippingCost; }
        public decimal? Taxes { get => SubTotal * Constants.TAXES_PERCENTAGE; }
        public string? Total { get => string.Format(new CultureInfo("fr-CA"), "{0:C}", SubTotal + Taxes); }
        public List<ShoppingCartProductVM>? Items { get; set; }
    }
}
