using Microsoft.Extensions.Logging;

namespace Catalogue.Application.Extensions;

public static class LoggerExtension
{
    public static void LogAndThrow<T> (this ILogger<T> logger, string exceptionMessage, string logMessage)
    {
        logger.LogError(logMessage);
        throw new ArgumentNullException(exceptionMessage);
    }
}
