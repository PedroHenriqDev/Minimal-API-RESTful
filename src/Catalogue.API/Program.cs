using Catalogue.API.Endpoints;
using Catalogue.API.Extensions;
using Catalogue.CrossCutting.AppDependencies;

namespace Catalogue.API;

public class Progam
{
    public static WebApplication CreateHost(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        IConfiguration cfg = builder.Configuration;

        cfg.LoadEnv();

        // Add services to the container.
        builder.Services.AddApiSwagger()
                        .ConfigureApiOptions()
                        .AddPersistence(cfg)
                        .AddApiAuthentication(cfg)
                        .AddApiAuthorization()
                        .AddDependencies()
                        .AddApiDefaultCors()
                        .AddApiServicesScoped()
                        .AddEndpointsApiExplorer()
                        .AddVersioning();

        WebApplication app = builder.Build();
        RouteGroupBuilder appVersioned = app.UseApiVersioned().WithTags();

        #region Endpoints
        //Middlewares


        //Categories Endpoints
        appVersioned.MapCategoriesEndpoints();

        //Products Endpoints
        appVersioned.MapProductsEndpoints();

        //Authentication Endpoints
        appVersioned.MapAuthEndpoints();

        #endregion

        //Middlewares
        app.UseSwaggerMiddleware()
           .UseExceptionHandling(builder.Environment)
           .UseCors()
           .UseAuthentication()
           .UseAuthorization()
           .UseGlobalExceptionFilter()
           .UseHttpsRedirection();

        return app;
    }

    public static void Main(string[] args)
    {
        var app = CreateHost(args);
        app.Run();
    }
}