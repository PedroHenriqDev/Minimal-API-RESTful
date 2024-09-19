using AutoBogus;
using Catalogue.API;
using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Extensions;
using Catalogue.Application.Interfaces.Services;
using Catalogue.Application.Utils;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Enums;
using Catalogue.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalogue.IntegrationTests.Fixtures;

public class CustomWebAppFixture : WebApplicationFactory<Progam>
{
    public AppDbContext? DbContext {get; private set;} 
    public User? Admin {get; set;}
    public ITokenService _TokenService = null!;
    public IClaimService _ClaimService = null!;
    public IConfiguration configuration = null!;

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

            configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            string? connectionString = configuration.GetConnectionString("TestConnection");
            
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDatabase"));
           
            DbContext = services.BuildServiceProvider().GetRequiredService<AppDbContext>();
            _TokenService = services.BuildServiceProvider().GetRequiredService<ITokenService>();
            _ClaimService = services.BuildServiceProvider().GetRequiredService<IClaimService>();

            Admin = new AutoFaker<User>().RuleFor(u => u.Role, f => Role.Admin).Generate();
            Admin.Password = Crypto.Encrypt(Admin.Password);
            DbContext.Users.Add(Admin);
        
        });
    }

    public string GenerateToken(string name, Role role)
    {
        var authClaims = _ClaimService.CreateAuthClaims(new UserResponse{ Name = name });
        authClaims.AddRole(role.ToString());
        return _TokenService.GenerateToken(authClaims, configuration);
    }
}