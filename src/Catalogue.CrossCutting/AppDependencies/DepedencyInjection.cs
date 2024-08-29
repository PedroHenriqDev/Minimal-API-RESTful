﻿using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Mappings.AutoMapper.Profiles;
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
            ?? throw new ArgumentNullException(nameof(connectionString));
     
        svc.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        svc.AddScoped<IUnitOfWork, UnitOfWork>();

        svc.AddFluentValidationAutoValidation();
        svc.AddValidatorsFromAssemblyContaining<CreateCategoryCommandRequest>();

        string? corsPolicyName = configuration["Cors:PolicyName"]
         ?? throw new ArgumentNullException(nameof(corsPolicyName));

        svc.AddCors(opt =>
        {
            opt.AddPolicy(corsPolicyName, policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });

        svc.AddAutoMapper(typeof(MappingProfile));

        var myHandlers = AppDomain.CurrentDomain.Load("Catalogue.Application");
        svc.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(myHandlers));

        return svc;
    }
}
