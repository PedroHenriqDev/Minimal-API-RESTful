using Catalogue.API.Endpoints;
using Catalogue.API.Extensions;
using Catalogue.API.Middlewares;
using Catalogue.CrossCutting.AppDependencies;

namespace Catalogue.API;

public class Progam
{
    public static WebApplication CreateHost(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        IConfiguration cfg = builder    .Configuration;

        cfg.LoadEnv();

        builder.Services
            .AddApiSwagger()
            .AddGzipResponseCompression()
            .ConfigureApiOptions()
            .AddPersistence(cfg)
            .AddApiAuthentication(cfg)
            .AddApiAuthorization()
            .AddDependencies()
            .AddApiDefaultCors()
            .AddGlobalException()
            .AddEndpointsApiExplorer()
            .AddVersioning();

        WebApplication app = builder.Build();
        RouteGroupBuilder appVersioned = app.UseApiVersioned();

        #region Map Endpoints

        appVersioned.MapCategoriesEndpoints();
        appVersioned.MapProductsEndpoints();
        appVersioned.MapAuthEndpoints();

        #endregion

        //Middlewares
        app.UseSwaggerMiddleware()
           .UseCompressionMiddleware()
           .UseExceptionHandling(builder.Environment)
           .UseCors()
           .UseAuthentication()
           .UseAuthorization()
           .UseHttpsRedirection()
           .UseGlobalExceptionMiddleware();

        return app;
    }

    public static void Main(string[] args)
    {
        var app = CreateHost(args);
        app.Run();
    }
}