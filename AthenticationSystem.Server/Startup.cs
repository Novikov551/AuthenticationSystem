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
               // ���������, ����� �� �������������� �������� ��� ��������� ������
               ValidateIssuer = true,
               // ������, �������������� ��������
               ValidIssuer = AuthOptions.ISSUER,
               // ����� �� �������������� ����������� ������
               ValidateAudience = true,
               // ��������� ����������� ������
               ValidAudience = AuthOptions.AUDIENCE,
               // ����� �� �������������� ����� �������������
               ValidateLifetime = true,
               // ��������� ����� ������������
               IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
               // ��������� ����� ������������
               ValidateIssuerSigningKey = true,
           };
       });

        services.AddAuthorization();// ���������� �������� �����������
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