using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AuthenticationSystem.Client.Models.Login
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "enter your surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "enter email")]
        [DataType(DataType.Password)]
        public string Email { get; set; }

        [Required(ErrorMessage = "enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "repeat password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "passwords must match")]
        public string PasswordConfirm { get; set; }

        public static RegistrationViewModel CreateEmpty()
        {
            return new RegistrationViewModel
            {
                Email = string.Empty,
                Password = string.Empty,
                Name = string.Empty,
                Surname = string.Empty,
            };
        }
    }

    public class RegistrationViewModelValidator : AbstractValidator<RegistrationViewModel>
    {
        public RegistrationViewModelValidator()
        {
            RuleFor(p => p.Email).EmailAddress();
            RuleFor(p => p.Password).Matches(new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"));
            RuleFor(p => p.Name).MinimumLength(2);
            RuleFor(p => p.Surname).MinimumLength(2);
        }
    }
}
