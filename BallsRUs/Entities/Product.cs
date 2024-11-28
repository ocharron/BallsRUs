using System.Data.SqlTypes;

namespace BallsRUs.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string? SKU { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? ImagePath { get; set; }
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public int? WeightInGrams { get; set; }
        public string? Size { get; set; }
        public int? Quantity { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public DateTime? PublicationDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsArchived { get; set; }

        // Navigation properties
        public List<Category> Categories { get; set; } = new();
    }
}