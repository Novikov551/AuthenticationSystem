using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AuthenticationSystem.Client.Models.Login
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public static LoginViewModel CreateEmpty()
        {
            return new LoginViewModel
            {
                Email = string.Empty,
                Password = string.Empty
            };
        }
    }

    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(p => p.Email).EmailAddress();
        }
    }
}
