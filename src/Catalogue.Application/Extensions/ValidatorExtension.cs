using Catalogue.Application.Exceptions;
using FluentValidation;

namespace Catalogue.Application.FluentValidation;

public static class ValidatorExtension 
{
    public static void EnsureValid<T>(this IValidator<T> validator, T request)
    {
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
