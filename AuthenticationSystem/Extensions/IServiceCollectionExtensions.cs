using AuthenticationSystem.Domain;
using AuthenticationSystem.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationSystem.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services,
           IConfiguration configuration,
           IHostEnvironment environment)
        {
            services.AddRepositoryContext(configuration, environment);

        }

        private static IServiceCollection AddRepositoryContext(this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
        {
            services.AddDbContext<AuthenticationSystemDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

                if (environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            services.AddScoped<IUnitOfWork>(s => s.GetService<AuthenticationSystemDbContext>()!);

            return services;
        }
    }
}
