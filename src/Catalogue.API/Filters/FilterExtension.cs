using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalogue.API.Filters;

public static class FilterExtension
{
    public static void UseGlobalExceptionFilter(this WebApplication app)
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
    }
}
