using AuthenticationSystem.Extensions;
using AuthenticationSystem.Logic.Users;
using AuthenticationSystem.Services;
using AuthenticationSystem.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Stemy.Dairy.FactStash.Services;
using UserAuthenticationSystem.Client.Auth;
using UserAuthenticationSystem.Domain.Core;

namespace AuthenticationSystem;

public class Startup
{
    public Startup(IConfiguration configuration, IHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public IConfiguration Configuration { get; }

    public IHostEnvironment Environment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Add services to the container.
        services.AddInfrastructureServices(Configuration, Environment);
        services.AddScoped<IUserManager<User,CreateUserRequest>, UserManager>();
        services.AddSingleton<SeederService>();

        services.AddAuthentication();
        services.AddAuthorization();// добавление сервисов авторизации

        services.AddHostedService<StartService>();
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}