namespace Catalogue.Domain.Validation;

public class DomainValidation : Exception
{
    public DomainValidation(string? message) : base(message)
    {
    }

    public static void When(bool hasError, string errorMessage)
    {
        if (hasError)
            throw new DomainValidation(errorMessage);
    }
}
