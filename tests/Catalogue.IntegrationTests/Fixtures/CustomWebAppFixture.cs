using Catalogue.API;
using Catalogue.Application.Utils;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Enums;
using Catalogue.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalogue.IntegrationTests.Fixtures;

public class CustomWebAppFixture : WebApplicationFactory<Progam>
{
    public AppDbContext? DbContext {get; private set;} 
    public User? Admin {get; set;}

   protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            string configPath = FixturesResource.CONFIG_PATH;
            config.AddJsonFile(configPath, optional: false, reloadOnChange: true);
        });

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            string? connectionString = configuration.GetConnectionString("TestConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

           
            DbContext = services.BuildServiceProvider().GetRequiredService<AppDbContext>();
            Admin = DbContext.Users.FirstOrDefault(u => u.Role == Role.Admin);
            if (Admin != null)
            {
                Admin.Password = Crypto.Decrypt(Admin.Password);
            }
        });
    }
}