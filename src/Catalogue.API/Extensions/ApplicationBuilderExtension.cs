using Catalogue.API.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalogue.API.Extensions;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
    {
        return app.UseSwagger()
                  .UseSwaggerUI();
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

    public static IApplicationBuilder UseGlobalExcetionFilter(this IApplicationBuilder app) 
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
