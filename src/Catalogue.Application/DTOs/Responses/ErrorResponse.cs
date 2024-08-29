namespace Catalogue.Application.DTOs.Responses;

public class ErrorResponse
{
    public IList<string> ErrorMessages { get; private set; } = [];

    public ErrorResponse(IList<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}
