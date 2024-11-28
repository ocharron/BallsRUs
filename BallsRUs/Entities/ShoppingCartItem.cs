using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BallsRUs.Entities
{
    public class ShoppingCartItem
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        // Foreign keys
        public Guid ShoppingCartId { get; set; }
        public Guid ProductId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }
        [ForeignKey(nameof(ShoppingCartId))]
        public virtual ShoppingCart? ShoppingCart { get; set; }
    }
}
