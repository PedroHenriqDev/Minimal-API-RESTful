using Catalogue.API.Resources;
using Catalogue.Application.Extensions;
using MediatR;
using System.Reflection;

namespace Catalogue.API.Filters;

public class InjectIdFilter : IEndpointFilter
{
    private readonly ILogger<InjectIdFilter> _logger;

    public InjectIdFilter(ILogger<InjectIdFilter> logger)
    {
        _logger = logger;
    }

    // Request parameter must contain a property named 'Id'.
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<IRequest<object>>().FirstOrDefault();

        if (request == null)
        {
            _logger.LogAndThrow(ApiErrorMessagesResource.REQUEST_NULL_FILTER_ERROR, nameof(context));
        }

        context.HttpContext.Request.RouteValues.TryGetValue("id", out var idValue);

        // Get property type with name is 'id'
        PropertyInfo? idProperty = request!.GetType().GetProperties()
                   .FirstOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));

        if (idProperty == null)
        {
            _logger.LogAndThrow(ApiErrorMessagesResource.REQUEST_NULL_FILTER_ERROR, nameof(idProperty));
        }

        // Try set property in request 
        if (idProperty!.PropertyType != typeof(Guid))
        {
            string message = string.Format
              (
                  ApiErrorMessagesResource.ID_TYPE_ERROR,
                  idProperty!.PropertyType,
                  idValue!.GetType()
              );

            _logger.LogAndThrow(message, message);
        }

        idProperty!.SetValue(request, Guid.Parse(idValue!.ToString()!));

        return await next(context);
    }
}
