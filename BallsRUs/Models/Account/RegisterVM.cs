using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.IO;
using System.Text.RegularExpressions;
using BallsRUs.Utilities;
using FluentValidation;

namespace BallsRUs.Models.Account
{
    public class RegisterVM
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? UserName { get; set; }

        public string? PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        public string? PasswordConfirmation { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public string? StateProvince { get; set; }

        public bool addAddress { get; set; }
    }
    public class RegisterVMValidator : AbstractValidator<RegisterVM>
    {
        public RegisterVMValidator()
        {
            RuleFor(vm => vm.UserName)
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
                    .WithMessage("Veuillez entrer votre numéro de téléphone")
                .Matches(Constants.PHONE_NUMBER_REGEX)
                    .WithMessage("Entrez seulement les chiffres de votre numéro de téléphone.")
                .MinimumLength(10)
                    .WithMessage("Entrez un numéro de téléphone valide.")
                .MaximumLength(11)
                    .WithMessage("Entrez un numéro de téléphone valide.");
            RuleFor(vm => vm.Password)
                .NotEmpty()
                    .WithMessage("Veuillez entrer un mot de passe");
            RuleFor(vm => vm.PasswordConfirmation)
                .NotEmpty()
                    .WithMessage("Veuillez confirmer votre mot de passe.")
                .Equal(vm => vm.Password)
                    .WithMessage("Le mot de passe et le mot de passe de confirmation de correspondent pas.");

            When(vm => vm.addAddress, () =>
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
            });
        }
    }
}
