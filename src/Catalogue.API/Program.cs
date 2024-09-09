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
                        .AddPersistence(cfg)
                        .AddApiAuthentication(cfg)
                        .AddApiAuthorization()
                        .AddDependencies()
                        .AddApiDefaultCors()
                        .AddApiServicesScoped()
                        .AddEndpointsApiExplorer();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        #region Endpoints
        //Categories Endpoints
        app.MapGetCategoriesEndpoints();
        app.MapPostCategoriesEndpoints();
        app.MapDeleteCategoriesEndpoints();
        app.MapPutCategoriesEndpoints();

        //Products Endpoints
        app.MapGetProductsEndpoints();
        app.MapPostProductsEndpoints();
        app.MapDeleteProductsEndpoints();
        app.MapPutProductsEndpoints();

        //Authentication Endpoints
        app.MapPostAuthEndpoints();
        app.MapPutAuthEndpoints();

        #endregion

        //Middlewares
        app.UseSwaggerMiddleware()
           .UseExceptionHandling(builder.Environment)
           .UseCors()
           .UseAuthentication()
           .UseAuthorization()
           .UseGlobalExcetionFilter()
           .UseHttpsRedirection();

        return app;
    }

    public static void Main(string[] args)
    {
        var app = CreateHost(args);
        app.Run();
    }
}