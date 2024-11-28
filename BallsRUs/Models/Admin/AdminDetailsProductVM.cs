using System.ComponentModel.DataAnnotations;

namespace BallsRUs.Models.Admin
{
    public class AdminDetailsProductVM
    {
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Display(Name = "SKU")]
        public string? SKU { get; set; }

        [Display(Name = "Nom")]
        public string? Name { get; set; }

        [Display(Name = "Marque")]
        public string? Brand { get; set; }

        [Display(Name = "Modèle")]
        public string? Model { get; set; }

        [Display(Name = "Catégorie")]
        public string? Category { get; set; }

        [Display(Name = "Fichier image")]
        public string? ImagePath { get; set; }

        [Display(Name = "Poids en grammes")]
        public int? WeightInGrams { get; set; }

        [Display(Name = "Taille")]
        public string? Size { get; set; }

        [Display(Name = "Quantités")]
        public int? Quantity { get; set; }

        [Display(Name = "Prix")]
        public string? RetailPrice { get; set; }

        [Display(Name = "Prix en rabais")]
        public string? DiscountedPrice { get; set; }

        [Display(Name = "Mise en ligne")]
        public string? PublicationDate { get; set; }

        [Display(Name = "Dernière modification")]
        public string? LastUpdated { get; set; }

        [Display(Name = "Archivé")]
        public bool IsArchived { get; set; }
    }
}
