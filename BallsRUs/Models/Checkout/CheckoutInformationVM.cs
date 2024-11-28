using BallsRUs.Utilities;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace BallsRUs.Models.Checkout
{
    public class CheckoutInformationVM
    {
        [Display(Name = "Prénom")]
        public string? FirstName { get; set; }

        [Display(Name = "Nom")]
        public string? LastName { get; set; }

        [Display(Name = "Adresse courriel")]
        public string? EmailAddress { get; set; }

        [Display(Name = "Téléphone")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Rue")]
        public string? AddressStreet { get; set; }

        [Display(Name = "Ville")]
        public string? AddressCity { get; set; }

        [Display(Name = "Province/État")]
        public string? AddressStateProvince { get; set; }

        [Display(Name = "Pays")]
        public string? AddressCountry { get; set; }

        [Display(Name = "Code postal")]
        public string? AddressPostalCode { get; set; }

        public bool HasExistingAddress { get; set; } = false;
        public bool UseExistingAddress { get; set; } = false;
        public bool SaveAddress { get; set; } = false;
        public bool ConfirmInformation { get; set; } = false;
    }

    public class CheckoutInformationValidator : AbstractValidator<CheckoutInformationVM>
    {
        public CheckoutInformationValidator()
        {
            RuleFor(vm => vm.FirstName)
                .NotEmpty()
                    .WithMessage("Veuillez entrer votre prénom.");
            RuleFor(vm => vm.LastName)
                .NotEmpty()
                    .WithMessage("Veuillez entrer votre nom.");
            RuleFor(vm => vm.EmailAddress)
                .NotEmpty()
                    .WithMessage("Veuillez entrer votre adresse courriel.")
                .EmailAddress()
                    .WithMessage("Veuillez entrer une adresse courriel valide.");
            RuleFor(vm => vm.PhoneNumber)
                .NotEmpty()
                    .WithMessage("Veuillez entrer votre numéro de téléphone")
                .Matches(Constants.PHONE_NUMBER_REGEX)
                    .WithMessage("Entrez seulement les chiffres de votre numéro de téléphone.")
                .MinimumLength(10)
                    .WithMessage("Entrez un numéro de téléphone valide.")
                .MaximumLength(11)
                    .WithMessage("Entrez un numéro de téléphone valide.");

            When(vm => !vm.UseExistingAddress, () =>
            {
                RuleFor(vm => vm.AddressStreet)
                    .NotEmpty()
                        .WithMessage("Veuillez entrer la rue de l'adresse de livraison");
                RuleFor(vm => vm.AddressCity)
                    .NotEmpty()
                        .WithMessage("Veuillez entrer la ville de l'adresse de livraison");
                RuleFor(vm => vm.AddressStateProvince)
                    .NotEmpty()
                        .WithMessage("Veuillez entrer la province ou l'état de l'adresse de livraison");
                RuleFor(vm => vm.AddressCountry)
                    .NotEmpty()
                        .WithMessage("Veuillez entrer le pays de l'adresse de livraison");
                RuleFor(vm => vm.AddressPostalCode)
                    .NotEmpty()
                        .WithMessage("Veuillez entrer le code postal de l'adresse de livraison")
                    .Matches(Constants.POSTAL_CODE_REGEX)
                        .WithMessage("Veuillez entrer un code postal valide. (Formats attendus: A1A 1A1 ou A1A1A1)");
            });

            RuleFor(vm => vm.ConfirmInformation)
                .Equal(true)
                    .WithMessage("Veuillez confirmer que les informations sont exactes avant de continuer.");
        }
    }
}
