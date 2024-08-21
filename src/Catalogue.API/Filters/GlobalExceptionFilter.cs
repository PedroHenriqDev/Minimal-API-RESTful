﻿using Catalogue.Application.DTOs;
using Catalogue.Application.Exceptions.Abstractions;
using Catalogue.Application.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalogue.API.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception.Message);

        if (context.Exception is ExceptionBase) 
        {
            var exception = (ExceptionBase)context.Exception;
            context.HttpContext.Response.StatusCode = (int)exception.GetStatusCodes();

            var result = new ErrorsDto(exception.GetMessages());
            context.HttpContext.Response.WriteAsJsonAsync(result);
        }
        else 
        {
            context.HttpContext.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;

            var result = new ErrorsDto(new List<string>
            {
                ErrorMessagesResource.SERVER_ERROR_MESSAGE
            });

            context.Result = new ObjectResult(result);
            context.HttpContext.Response.WriteAsJsonAsync(result);
        }
    }
}