using AuthenticationSystem.Client.Config;
using AuthenticationSystem.Client.Services;
using AuthenticationSystem.Client.Services.Notification.Messages;
using AuthenticationSystem.Client.Services.Rest;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthenticationSystem.Client.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddConfigOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthApiConfig>(configuration.GetSection(nameof(AuthApiConfig)));
        }

        public static IServiceCollection AddExternalApiService(this IServiceCollection services)
        {
            services.AddHttpClient<IAuthApiService, AuthApiService>()
                .AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
            services.AddScoped<IAuthApiService, AuthApiService>();

            return services;
        }

        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Account/Login";
                        options.AccessDeniedPath = "/Base/AccessDenied";
                        options.LogoutPath = "/Account/Logout";
                        options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    });
            services.AddAuthorization()// добавление сервисов авторизации
                .AddScoped<IAuthorizationService, AuthorizationService>()
                .AddHttpContextAccessor();

            return services;
        }

        public static void AddNotifyServices(this IServiceCollection services)
        {
            services.AddSingleton<NotifyService>();
        }
    }
}
