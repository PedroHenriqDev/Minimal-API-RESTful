﻿using Catalogue.Application.Exceptions.Abstractions;
using System.Net;

namespace Catalogue.Application.Exceptions;

public class HttpInvalidQueryException : ExceptionBase
{
    public HttpInvalidQueryException(string message) : base(message) 
    {
    }

    public override IList<string> GetMessages()
    {
        return [Message];
    }

    public override HttpStatusCode GetStatusCodes()
    {
        return HttpStatusCode.BadRequest;
    }
}