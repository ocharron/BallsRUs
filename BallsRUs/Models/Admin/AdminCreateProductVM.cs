using BallsRUs.Utilities;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace BallsRUs.Models.Admin
{
    public class AdminCreateProductVM
    {
        [Display(Name = "SKU")]
        public string? SKU { get; set; }

        [Display(Name = "Nom")]
        public string? Name { get; set; }

        [Display(Name = "Marque")]
        public string? Brand { get; set; }

        [Display(Name = "Modèle")]
        public string? Model { get; set; }

        [Display(Name = "Catégorie sélectionnée")]
        public string? SelectedCategory { get; set; }

        [Display(Name = "Catégorie")]
        public List<string>? Categories { get; set; }

        [Display(Name = "Fichier image")]
        public IFormFile? Image { get; set; }

        [Display(Name = "Description courte")]
        public string? ShortDescription { get; set; }

        [Display(Name = "Description complète")]
        public string? FullDescription { get; set; }

        [Display(Name = "Poids en grammes")]
        public int? WeightInGrams { get; set; }

        [Display(Name = "Taille")]
        public string? Size { get; set; }

        [Display(Name = "Quantités")]
        public int? Quantity { get; set; }

        [Display(Name = "Prix")]
        public decimal? RetailPrice { get; set; }

        [Display(Name = "Prix en rabais (si en rabais)")]
        public decimal? DiscountedPrice { get; set; }

        public class Validator : AbstractValidator<AdminCreateProductVM>
        {
            public Validator()
            {
                RuleFor(vm => vm.SKU)
                    .NotEmpty()
                        .WithMessage("Veuillez entrer un SKU.")
                    .Matches(Constants.PRODUCT_SKU_REGEX)
                        .WithMessage("Veuillez entrer un SKU valide. Assurez-vous de respecter le format suivant: ABC1234DEF. " +
                                     "(ABC est la catégorie, 1234 le numéro de modèle et DEF est la marque)");
                RuleFor(vm => vm.Name)
                    .NotEmpty()
                        .WithMessage("Veuillez entrer un nom.")
                    .Length(Constants.PRODUCT_MIN_NAME_LENGTH, Constants.PRODUCT_MAX_NAME_LENGTH)
                        .WithMessage($"Veuillez entrer un nom entre {Constants.PRODUCT_MIN_NAME_LENGTH} et {Constants.PRODUCT_MAX_NAME_LENGTH} caractères.");
                RuleFor(vm => vm.Brand)
                    .NotEmpty()
                        .WithMessage("Veuillez entrer un nom de marque.")
                    .Length(Constants.PRODUCT_MIN_NAME_LENGTH, Constants.PRODUCT_MAX_NAME_LENGTH)
                        .WithMessage($"Veuillez entrer un nom de marque entre {Constants.PRODUCT_MIN_NAME_LENGTH} et {Constants.PRODUCT_MAX_NAME_LENGTH} caractères.");
                RuleFor(vm => vm.Model)
                    .NotEmpty()
                        .WithMessage("Veuillez entrer un modèle.")
                    .Length(Constants.PRODUCT_MIN_NAME_LENGTH, Constants.PRODUCT_MAX_NAME_LENGTH)
                        .WithMessage($"Veuillez entrer un modèle entre {Constants.PRODUCT_MIN_NAME_LENGTH} et {Constants.PRODUCT_MAX_NAME_LENGTH} caractères.");
                RuleFor(vm => vm.SelectedCategory)
                    .NotEmpty()
                        .WithMessage("Veuillez choisir une catégorie.");
                RuleFor(vm => vm.Image)
                    .NotEmpty()
                        .WithMessage("Veuillez sélectionner une image.");
                RuleFor(vm => vm.ShortDescription)
                    .NotEmpty()
                        .WithMessage("Veuillez entrer une description courte.")
                    .Length(Constants.PRODUCT_MIN_SHORT_DESC_LENGTH, Constants.PRODUCT_MAX_SHORT_DESC_LENGTH)
                        .WithMessage($"Veuillez entrer une description courte entre {Constants.PRODUCT_MIN_SHORT_DESC_LENGTH} et {Constants.PRODUCT_MAX_SHORT_DESC_LENGTH} caractères.");
                RuleFor(vm => vm.FullDescription)
                    .NotEmpty()
                        .WithMessage("Veuillez entrer une description complète.")
                    .Length(Constants.PRODUCT_MIN_FULL_DESC_LENGTH, Constants.PRODUCT_MAX_FULL_DESC_LENGTH)
                        .WithMessage($"Veuillez entrer une description complète entre {Constants.PRODUCT_MIN_FULL_DESC_LENGTH} et {Constants.PRODUCT_MAX_FULL_DESC_LENGTH} caractères.");
                RuleFor(vm => vm.WeightInGrams)
                    .GreaterThan(0)
                        .When(x => x.WeightInGrams is not null)
                            .WithMessage("Veuillez entrer un poids plus grand que zéro.");
                RuleFor(vm => vm.Quantity)
                    .NotEmpty()
                        .WithMessage("Veuillez entrer une quantité.")
                    .GreaterThanOrEqualTo(0)
                        .WithMessage("Veuillez entrer une quantité positive.");
                RuleFor(vm => vm.RetailPrice)
                    .NotEmpty()
                        .WithMessage("Veuillez entrer un prix.")
                    .GreaterThan(0)
                        .WithMessage("Veuillez entrer une prix plus grand que zéro.");
                RuleFor(vm => vm.DiscountedPrice)
                    .GreaterThan(0)
                        .When(x => x.DiscountedPrice is not null)
                            .WithMessage("Veuillez entrer un prix en rabais plus grand que zéro.")
                    .LessThan(x => x.RetailPrice)
                        .When(x => x.DiscountedPrice is not null)
                            .WithMessage("Veuillez entrer un prix inférieur à celui de base.");
            }
        }
    }
}
