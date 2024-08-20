using Catalogue.Application.Exceptions.Abstractions;
using System.Net;

namespace Catalogue.Application.Exceptions;

public class ErrorOnValidationException : ExceptionBase
{
    private readonly IList<string> _errorMessages = [];

    public ErrorOnValidationException(IList<string> errorMessages) : base(string.Empty)
    {
        _errorMessages = errorMessages; 
    }

    public override IList<string> GetMessages()
    {
        return _errorMessages;
    }

    public override HttpStatusCode GetStatusCodes()
    {
        return HttpStatusCode.BadRequest;
    }
}
