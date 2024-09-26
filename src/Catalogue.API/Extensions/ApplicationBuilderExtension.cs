using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Catalogue.API.Middlewares;

namespace Catalogue.API.Extensions;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseSwaggerMiddleware(this WebApplication app)
    {
        return app.UseSwagger()
                  .UseSwaggerUI(options => 
                  {
                      IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

                      foreach(ApiVersionDescription description in descriptions) 
                      {
                          string url = $"/swagger/{description.GroupName}/swagger.json";
                          string name = description.GroupName.ToUpperInvariant();

                          options.SwaggerEndpoint(url, name);
                      }
                  });
    }

    public static RouteGroupBuilder UseApiVersioned(this WebApplication app)
    {
        ApiVersionSet versionSet = app.NewApiVersionSet()
            .ReportApiVersions()
            .HasApiVersion(new ApiVersion(1))
            .Build();

        RouteGroupBuilder appVersioned = app.MapGroup("api/v{apiVersion:apiVersion}/")
                                            .WithApiVersionSet(versionSet);
        return appVersioned;
    }

    public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }

    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app,
                                                           IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        return app;
    }

    public static IApplicationBuilder UseCompressionMiddleware(this IApplicationBuilder app)
    {
        return app.UseResponseCompression();
    }
}
