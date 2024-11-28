using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BallsRUs.Entities
{
    public class ProductCategory
    {
        [Key]
        [Column(Order = 0)]
        public Guid ProductId { get; set; }
        [Key]
        [Column(Order = 1)]
        public Guid CategoryId { get; set; }

        // Foreign keys
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
    }
}
