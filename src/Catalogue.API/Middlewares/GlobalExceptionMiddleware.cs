using Catalogue.API.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalogue.API.Middlewares;

public class GlobalExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        GlobalExceptionFilter expectionFilter = context.RequestServices.GetRequiredService<GlobalExceptionFilter>();

        try
        {
            await next(context);
        }
        catch(Exception ex)
        {
            var actionContext = new ActionContext(context, new RouteData(), new ActionDescriptor());
            var expectionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = ex
            };
            expectionFilter.OnException(expectionContext);
        }
    }
}