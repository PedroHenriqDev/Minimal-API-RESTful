using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Exceptions.Abstractions;
using Catalogue.Application.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalogue.API.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private ErrorResponse response;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
        response = new ErrorResponse();
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception.Message);
        if (context.Exception is ExceptionBase) 
        {
            var exception = (ExceptionBase)context.Exception;
            context.HttpContext.Response.StatusCode = (int)exception.GetStatusCodes();

            response = new ErrorResponse(exception.GetMessages());
            context.HttpContext.Response.WriteAsJsonAsync(response);
        }
        else 
        {
            context.HttpContext.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;

            response = new ErrorResponse(new List<string>
            {
                ErrorMessagesResource.SERVER_ERROR_MESSAGE
            });

            context.Result = new ObjectResult(response);
            context.HttpContext.Response.WriteAsJsonAsync(response);
        }
    }
}
