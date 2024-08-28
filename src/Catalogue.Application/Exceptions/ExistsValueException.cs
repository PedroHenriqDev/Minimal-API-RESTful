using Catalogue.Application.Exceptions.Abstractions;
using System.Net;

namespace Catalogue.Application.Exceptions;

public class ExistsValueException : ExceptionBase
{

    public ExistsValueException(string message) : base(message) 
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
