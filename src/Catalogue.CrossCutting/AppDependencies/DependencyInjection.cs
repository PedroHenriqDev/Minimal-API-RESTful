using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Interfaces.Services;
using Catalogue.Application.Mappings.AutoMapper.Profiles;
using Catalogue.Application.Services;
using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
namespace Catalogue.CrossCutting.AppDependencies;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<IStatisticsService, StatisticsService>();

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateCategoryCommandRequest>();

        services.AddAutoMapper(typeof(MappingProfile));

        var myHandlers = AppDomain.CurrentDomain.Load("Catalogue.Application");
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(myHandlers));

        return services;
    }
}
