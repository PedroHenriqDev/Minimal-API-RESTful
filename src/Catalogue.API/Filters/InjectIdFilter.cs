using Catalogue.API.Resources;
using MediatR;

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

        if (request is null)
        {
            LogAndThrow(nameof(context), ApiErrorMessagesResource.REQUEST_NULL_FILTER_ERROR);
        }

        context.HttpContext.Request.RouteValues.TryGetValue("id", out var idValue);

        // Get property type with name is 'id'
        var idProperty = request!.GetType().GetProperties()
                   .FirstOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));

        if (idProperty is null)
        {
            LogAndThrow(nameof(idProperty), ApiErrorMessagesResource.ID_PROPERTY_NULL_ERROR);
        }

        // Try set property in request 
        try
        {
            idProperty!.SetValue(request, Convert.ChangeType(idValue, idProperty.PropertyType));
        }
        catch (Exception ex) 
        {
            string logMessage = string.Format(ApiErrorMessagesResource.ID_PROPERTY_NOT_EQUAL_ERROR, idProperty!.PropertyType, idValue!.GetType());
            LogAndThrow(ex.Message, logMessage);
        }

        return await next(context);
    }

    private void LogAndThrow(string exceptionMessage, string logMessage)
    {
        _logger.LogError(logMessage);
        throw new ArgumentNullException(exceptionMessage);
    }
}
