using BallsRUs.Entities;
using System.ComponentModel.DataAnnotations;

namespace BallsRUs.Models.Product
{
    public class ProductDetailsVM
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        [Display(Name = "SKU")]
        public string? SKU { get; set; }

        [Display(Name = "Nom")]
        public string? Name { get; set; }

        [Display(Name = "Marque")]
        public string? Brand { get; set; }

        [Display(Name = "Modèle")]
        public string? Model { get; set; }

        [Display(Name = "Image")]
        public string? ImagePath { get; set; }

        [Display(Name = "Description courte")]
        public string? ShortDescription { get; set; }

        [Display(Name = "Description")]
        public string? FullDescription { get; set; }

        [Display(Name = "Poids (grammes)")]
        public int? WeightInGrams { get; set; }

        [Display(Name = "Tailles")]
        public string? Size { get; set; }

        [Display(Name = "Quantité")]
        public int? Quantity { get; set; }

        [Display(Name = "Prix")]
        public string? RetailPrice { get; set; }

        [Display(Name = "Prix réduit")]
        public string? DiscountedPrice { get; set; }

        [Display(Name = "Date de mise en ligne")]
        public string? PublicationDate { get; set; }

        public List<Category>? Categories { get; set; }
    }
}
