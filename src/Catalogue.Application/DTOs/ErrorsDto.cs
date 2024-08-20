namespace Catalogue.Application.DTOs;

public class ErrorsDto
{
    public IList<string> ErrorMessages { get; private set; } = [];

    public ErrorsDto(IList<string> errorMessages)
    {
        this.ErrorMessages = errorMessages;
    }
}
