using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserAuthenticationSystem.Client.Auth;

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
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               // указывает, будет ли валидироваться издатель при валидации токена
               ValidateIssuer = true,
               // строка, представляющая издателя
               ValidIssuer = AuthOptions.ISSUER,
               // будет ли валидироваться потребитель токена
               ValidateAudience = true,
               // установка потребителя токена
               ValidAudience = AuthOptions.AUDIENCE,
               // будет ли валидироваться время существования
               ValidateLifetime = true,
               // установка ключа безопасности
               IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
               // валидация ключа безопасности
               ValidateIssuerSigningKey = true,
           };
       });

        services.AddAuthorization();// добавление сервисов авторизации
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