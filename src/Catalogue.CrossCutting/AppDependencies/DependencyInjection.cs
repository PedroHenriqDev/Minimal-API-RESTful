using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Interfaces.Services;
using Catalogue.Application.Mappings.AutoMapper.Profiles;
using Catalogue.Application.Services;
using Catalogue.Domain.Enums;
using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Context;
using Catalogue.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Catalogue.CrossCutting.AppDependencies;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection svc, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException(nameof(configuration));
     
        svc.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        svc.AddScoped<IUnitOfWork, UnitOfWork>();
        svc.AddScoped<ITokenService, TokenService>();
        svc.AddScoped<IClaimService, ClaimService>();

        svc.AddFluentValidationAutoValidation();
        svc.AddValidatorsFromAssemblyContaining<CreateCategoryCommandRequest>();

        string corsPolicyName = configuration["Cors:PolicyName"]
         ?? throw new ArgumentNullException(nameof(configuration));

        svc.AddCors(opt =>
        {
            opt.AddPolicy(corsPolicyName, policy =>
            {
                policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });

        string secretKey = configuration["Jwt:Secret"] ??
            throw new ArgumentNullException(nameof(secretKey));

        svc.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt => 
        {
            opt.RequireHttpsMetadata = false;
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        svc.AddAuthorization(opt => 
        {
            opt.AddPolicy(name: "AdminOnly", policy =>
                policy.RequireRole(Role.Admin.ToString()));
        });

        svc.AddAutoMapper(typeof(MappingProfile));

        var myHandlers = AppDomain.CurrentDomain.Load("Catalogue.Application");
        svc.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(myHandlers));

        return svc;
    }
}
