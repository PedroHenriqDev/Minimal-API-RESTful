using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Context;
using Catalogue.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalogue.CrossCutting.AppDependencies;

public static class DepedencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection svc, IConfiguration configuration) 
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException();

        svc.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        svc.AddScoped<IUnitOfWork, UnitOfWork>();

        return svc;
    }
}
