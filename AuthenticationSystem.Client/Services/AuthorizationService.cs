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

        public async Task<Result> LoginAsync(LoginViewModel loginInfo)
        {
            try
            {
                var result = await _authApiService.LoginAsync(new LoginRequest
                {
                    Email = loginInfo.Email,
                    Password = loginInfo.Password,
                });

                if (result.IsFailed)
                {
                    return result.ToResult();
                }

                await InitCookies(result.Value,
                    loginInfo.Email,
                    loginInfo.RememberMe);

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

                await InitCookies(regResult.Value,
                    registrationViewModel.Email,
                    false);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Result.Fail(ex.Message);
            }
        }

        private async Task InitCookies(UserResponse user,
            string email,
            bool rememberMe)
        {
            var roleByte = (byte)user.Role.RoleType;

            //добавляем данные о пользователе для cookies
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, roleByte.ToString())
            };

            // создаем объект ClaimsIdentity
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            if (rememberMe)
            {
                // установка аутентификационных cookies
                await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(7),
                });
            }
            else
            {
                // установка аутентификационных cookies
                await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }
        }
    }
}
