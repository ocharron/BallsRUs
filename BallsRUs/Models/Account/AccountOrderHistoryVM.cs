using BallsRUs.Entities;
using System.ComponentModel.DataAnnotations;

namespace BallsRUs.Models.Account
{
    public class AccountOrderHistoryVM
    {
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Display(Name = "# Commande")]
        public string? Number { get; set; }

        [Display(Name = "Statut")]
        public string? Status { get; set; }

        [Display(Name = "Nom complet")]
        public string? FullName { get; set; }
        
        [Display(Name = "Nombre d'articles")]
        public int ProductQuantity { get; set; }
        
        [Display(Name = "Total")]
        public string? Total { get; set; }

        [Display(Name = "Creation date")]
        public DateTime CreationDate { get; set; }
    }
}