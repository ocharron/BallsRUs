using BallsRUs.Utilities;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace BallsRUs.Models.Account
{
    public class AccountAddressVM
    {
        [Display(Name = "Rue")]
        public string? Street { get; set; }

        [Display(Name = "Ville")]
        public string? City { get; set; }

        [Display(Name = "Province/État")]
        public string? StateProvince { get; set; }

        [Display(Name = "Pays")]
        public string? Country { get; set; }

        [Display(Name = "Code Postal")]
        public string? PostalCode { get; set; }

        public bool HasExistingAddress { get; set; } = false;
    }

    public class AccountAddressVMValidator : AbstractValidator<AccountAddressVM>
    {
        public AccountAddressVMValidator()
        {
            RuleFor(vm => vm.Street)
                .NotEmpty()
                    .WithMessage("Veuillez entrer une rue.");
            RuleFor(vm => vm.City)
                .NotEmpty()
                    .WithMessage("Veuillez entrer une ville.");
            RuleFor(vm => vm.PostalCode)
                .NotEmpty()
                    .WithMessage("Veuillez entrer un code postal.")
                .Matches(Constants.POSTAL_CODE_REGEX)
                    .WithMessage("Veuillez entrer un code postal valide. (Formats attendus: A1A 1A1 ou A1A1A1)");
            RuleFor(vm => vm.Country)
                .NotEmpty()
                    .WithMessage("Veuillez entrer un pays.");
            RuleFor(vm => vm.StateProvince)
                .NotEmpty()
                    .WithMessage("Veuillez entrer une province ou un état.");
        }
    }
}