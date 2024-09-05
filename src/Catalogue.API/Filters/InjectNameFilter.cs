﻿using Catalogue.API.Resources;
using Catalogue.Application.Extensions;
using MediatR;
using System.Reflection;

namespace Catalogue.API.Filters;

public class InjectNameFilter : IEndpointFilter
{
    private readonly ILogger<InjectNameFilter> _logger;

    public InjectNameFilter(ILogger<InjectNameFilter> logger)
    {
        _logger = logger;
    }

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<IRequest<object>>().FirstOrDefault();
        
        if (request == null) 
        {
            _logger.LogAndThrow(nameof(request), ApiErrorMessagesResource.REQUEST_NULL_FILTER_ERROR);
        }

        PropertyInfo? nameProperty = request!.GetType()
            .GetProperties()
            .FirstOrDefault(p => p.Name.Equals("Name", StringComparison.OrdinalIgnoreCase));

        if(nameProperty == null) 
        {
            _logger.LogAndThrow(nameof(nameProperty), ApiErrorMessagesResource.REQUEST_NULL_FILTER_ERROR);
        }

        try 
        {
            nameProperty!.SetValue(request, context.HttpContext.User.Identity.Name);
        }
        catch(Exception ex) 
        {
            string message = string.Format
                (
                    ApiErrorMessagesResource.NAME_PROPERTY_NOT_EQUAL_ERROR,
                    nameProperty!.PropertyType,
                    context.HttpContext.User.Identity.Name!.GetType()
                );
            _logger.LogAndThrow(message, ex.Message);
        }

        return next(context);
    }
}