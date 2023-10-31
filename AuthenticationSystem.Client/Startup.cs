using AuthenticationSystem.Client.Extensions;
using AuthenticationSystem.Client.Services.Notification.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

namespace UserAuthenticationSystem.Client;

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
        // Add services to the container.
        services.AddExternalApiService();
        services.AddConfigOptions(Configuration);

        services.AddSingleton<FrontendCommunicationService>();

        services.ConfigureAuthentication();

        var mvcBuilder = services.AddControllersWithViews().AddNewtonsoftJson();

        var assembly = Assembly.GetAssembly(typeof(Startup));

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        services.AddValidatorsFromAssembly(assembly);

        services.AddNotifyServices();
        services.AddSignalR();

        if (Environment.IsDevelopment())
        {
            mvcBuilder.AddRazorRuntimeCompilation();
        }
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Index}");
        });
    }
}