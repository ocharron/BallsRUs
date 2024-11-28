using BallsRUs.Utilities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace BallsRUs.Entities
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }
        public int ProductsQuantity { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        // Foreign keys
        public Guid? UserId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}
