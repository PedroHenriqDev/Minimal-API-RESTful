using Catalogue.API.Filters;
using Catalogue.Domain.Enums;
using Catalogue.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Catalogue.API.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApiSwagger(this IServiceCollection services)
    {
        return services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalogue API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Use Bearer 'ey...'"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
        });
    }


    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
         ?? throw new ArgumentNullException(nameof(configuration));

        return services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
    }

    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration) 
    {
        string secretKey = configuration["Jwt:Secret"] ??
        throw new ArgumentNullException(nameof(secretKey));

        services.AddAuthentication(opt =>
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

        return services;
    }

    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        return services.AddAuthorization(opt =>
        {
            opt.AddPolicy(name: "AdminOnly", policy =>
                policy.RequireRole(Role.Admin.ToString()));
        });
    }

    public static IServiceCollection AddApiDefaultCors(this IServiceCollection services) 
    {
        return services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
    }

    public static IServiceCollection AddApiServicesScoped(this IServiceCollection services) 
    {
        return services.AddScoped<GlobalExceptionFilter>();
    }
}
