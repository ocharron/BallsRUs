using BallsRUs.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BallsRUs.Models.Admin
{
    public class AdminManageOrderVM
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        [Display(Name = "Numéro")]
        public string? Number { get; set; }

        [Display(Name = "Statut")]
        public OrderStatus? Status { get; set; }

        [Display(Name = "Prénom")]
        public string? FirstName { get; set; }

        [Display(Name = "Nom")]
        public string? LastName { get; set; }

        [Display(Name = "Courriel")]
        public string? EmailAddress { get; set; }

        [Display(Name = "Téléphone")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Nb Produits")]
        public int? ProductQuantity { get; set; }

        [Display(Name = "Sous-Total")]
        public string? SubTotal { get; set; }

        [Display(Name = "Total")]
        public string? Total { get; set; }

        [Display(Name = "Date Création")]
        public string? CreationDate { get; set; }
    }
}
