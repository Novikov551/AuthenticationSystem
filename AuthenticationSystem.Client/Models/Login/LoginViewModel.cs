using System.ComponentModel.DataAnnotations;

namespace AuthenticationSystem.Client.Models.Login
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public static LoginViewModel CreateEmpty()
        {
            return new LoginViewModel
            {
                Email = string.Empty,
                Password = string.Empty
            };
        }
    }

    //public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    //{
    //    public LoginViewModelValidator()
    //    {
    //        RuleFor(p => p.Email).EmailAddress();
    //        RuleFor(p => p.Password).MinimumLength(8).Matches("\"^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$\"");
    //    }
    //}
}
