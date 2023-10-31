using AuthenticationSystem.Client.Models.Login;
using AuthenticationSystem.Client.Services.Rest;
using AuthenticationSystem.Shared;
using FluentResults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace AuthenticationSystem.Client.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAuthApiService _authApiService;
        private readonly ILogger<AuthorizationService> _logger;

        public AuthorizationService(IAuthApiService authApiService,
            IHttpContextAccessor httpContext,
            ILogger<AuthorizationService> logger)
        {
            _authApiService = authApiService;
            _httpContext = httpContext;
            _logger = logger;
        }

        public async Task<Result> LoginAsync(string email, string password)
        {
            try
            {
                var result = await _authApiService.LoginAsync(new LoginRequest
                {
                    Email = email,
                    Password = password,
                });

                if (result.IsFailed)
                {
                    return result.ToResult();
                }

                await InitCookies(result.Value.Name, result.Value.Surname, email, result.Value.Id);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result> LogoutAsync()
        {
            try
            {
                await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result> RegisterAsync(RegistrationViewModel registrationViewModel)
        {
            try
            {
                var regResult = await _authApiService.RegisterAsync(new CreateUserRequest
                {
                    Email = registrationViewModel.Email,
                    Name = registrationViewModel.Name,
                    Password = registrationViewModel.Password,
                    Surname = registrationViewModel.Surname,
                });

                if (regResult.IsFailed)
                {
                    return regResult.ToResult();
                }

                await InitCookies(regResult.Value.Name, regResult.Value.Surname, registrationViewModel.Email, regResult.Value.Id);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Result.Fail(ex.Message);
            }
        }

        private async Task InitCookies(string name,
            string surname,
            string email,
            Guid userId)
        {
            //добавляем данные о пользователе для cookies
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim("UserId", userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Surname, surname),
            };

            // создаем объект ClaimsIdentity
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            // установка аутентификационных cookies
            await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
    }
}
