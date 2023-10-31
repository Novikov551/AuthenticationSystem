using AuthenticationSystem;
using AuthenticationSystem.Infrastructure.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Serilog;

public class Program
{
    public static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

        try
        {
            Log.Information("Application Starting");
            var host = CreateHostBuilder(args).Build();
            //using (var scope = host.Services.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<AuthenticationSystemDbContext>();
            //    await db.Database.MigrateAsync();
            //}

            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The Application failed to start");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

                config.AddEnvironmentVariables();
            })
            .UseSerilog((hostContext, loggerConfig) =>
            {
                loggerConfig
                    .ReadFrom.Configuration(hostContext.Configuration)
                    .Enrich.WithProperty("ApplicationName", hostContext.HostingEnvironment.ApplicationName);
            }).UseSerilog();
}
