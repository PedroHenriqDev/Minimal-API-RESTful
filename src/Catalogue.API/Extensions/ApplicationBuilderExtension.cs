using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Catalogue.API.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

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

    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app,
                                                           IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        return app;
    }

    public static IApplicationBuilder UseGlobalExceptionFilter(this IApplicationBuilder app) 
    {
        app.Use(async (context, next) =>
        {
            var exceptionFilter = context.RequestServices.GetRequiredService<GlobalExceptionFilter>();

            try
            {
                await next();
            }
            catch (Exception ex)
            {
                var actionContext = new ActionContext(context, new RouteData(), new ActionDescriptor());
                var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
                {
                    Exception = ex
                };

                exceptionFilter.OnException(exceptionContext);
            }
        });

        return app;
    }
}
