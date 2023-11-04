using AuthenticationSystem.Client.Models.Login;
using FluentResults;

namespace AuthenticationSystem.Client.Services
{
    public interface IAuthorizationService
    {
        public Task<Result> LoginAsync(LoginViewModel loginInfo);
        public Task<Result> RegisterAsync(RegistrationViewModel registrationViewModel);
        public Task<Result> LogoutAsync();
    }
}
