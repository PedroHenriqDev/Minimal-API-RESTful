using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Validations;
using Catalogue.Application.Mappings.Profiles;
using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Context;
using Catalogue.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalogue.CrossCutting.AppDependencies;

public static class DepedencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection svc, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException();

        svc.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        svc.AddScoped<IUnitOfWork, UnitOfWork>();
        svc.AddScoped<CreateCategoryCommandValidator>();
        svc.AddScoped<UpdateCategoryCommandValidator>();

        svc.AddFluentValidationAutoValidation();
        svc.AddValidatorsFromAssemblyContaining<CreateCategoryCommandRequest>();

        svc.AddAutoMapper(typeof(MappingProfile));

        var myHandlers = AppDomain.CurrentDomain.Load("Catalogue.Application");
        svc.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(myHandlers));

        return svc;
    }
}
