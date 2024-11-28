using BallsRUs.Utilities;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace BallsRUs.Models.Account
{
    public class AccountEditInfoVM
    {
        [Display(Name = "Prénom")]
        public string? FirstName { get; set; }

        [Display(Name = "Nom")]
        public string? LastName { get; set; }
        
        [Display(Name = "Courriel")]
        public string? Email { get; set; }
        
        [Display(Name = "Téléphone")]
        public string? PhoneNumber { get; set; }
    }

    public class AccountEditInfoVMValidator : AbstractValidator<AccountEditInfoVM>
    {
        public AccountEditInfoVMValidator()
        {
            RuleFor(vm => vm.Email)
                .NotEmpty()
                    .WithMessage("Veuillez entrer une adresse courriel.")
                .EmailAddress()
                    .WithMessage("Veuillez entrer une adresse courriel valide.");
            RuleFor(vm => vm.FirstName)
                 .NotEmpty()
                    .WithMessage("Veuillez entrer un prénom.");
            RuleFor(vm => vm.LastName)
                .NotEmpty()
                    .WithMessage("Veuillez entrer un nom.");
            RuleFor(vm => vm.PhoneNumber)
                .NotEmpty()
                    .WithMessage("Veuillez entrer un numéro.")
                .Matches(Constants.PHONE_NUMBER_REGEX)
                    .WithMessage("Le numéro ne doit contenir que des chiffres.");
        }
    }
}