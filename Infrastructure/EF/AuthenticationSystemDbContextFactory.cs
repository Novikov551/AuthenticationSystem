using System;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuthenticationSystem.Infrastructure.EF;

//public class AuthenticationSystemDbContextFactory : IDesignTimeDbContextFactory<AuthenticationSystemDbContext>
//{
//    private string? EnvName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

//    public AuthenticationSystemDbContext CreateDbContext(string[] args)
//    {
//        //var configuration = new ConfigurationBuilder()
//        //    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
//        //    .AddJsonFile("appsettings.json", optional: false)
//        //    .AddJsonFile($"appsettings.{EnvName}.json", optional: false)
//        //    .Build();

//        //var optionsBuilder = new DbContextOptionsBuilder<AuthenticationSystemDbContext>();
//        //optionsBuilder.UseNpgsql(configuration.GetConnectionString(nameof(AuthenticationSystemDbContext)));

//        return new AuthenticationSystemDbContext(/*optionsBuilder.Options*/);
//    }
//}