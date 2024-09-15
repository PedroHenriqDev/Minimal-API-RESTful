using Catalogue.API;
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
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            string configPath = FixturesResource.CONFIG_PATH;
            config.AddJsonFile(configPath, optional: false, reloadOnChange: true);
        });

        builder.ConfigureServices(services =>
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            ServiceDescriptor? descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if(descriptor != null)
            {
                services.Remove(descriptor);
            }

            string? connectionString = configuration.GetConnectionString("TestConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

            DbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        });
    }
}