using System.Net;

namespace Catalogue.Application.Exceptions.Abstractions;

public abstract class ExceptionBase : Exception
{
    public ExceptionBase(string message) : base(message) 
    {
    }

    public abstract IList<string> GetMessages();

    public abstract HttpStatusCode GetStatusCodes();
}
