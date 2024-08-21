using Catalogue.Application.Exceptions.Abstractions;
using System.Net;

namespace Catalogue.Application.Exceptions;

public class NotFoundException : ExceptionBase
{
    public NotFoundException(string message) : base(message)
    {
    }

    public override IList<string> GetMessages()
    {
        return [Message];
    }

    public override HttpStatusCode GetStatusCodes()
    {
        return HttpStatusCode.NotFound;
    }
}
