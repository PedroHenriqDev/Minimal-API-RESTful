using Catalogue.Application.Resources;
using FluentValidation;

namespace Catalogue.Application.Abstractions.Validators;

public class CategoryBaseValidator<T> : AbstractValidator<T> where T : CategoryBase
{
    public CategoryBaseValidator()
    {

        const int NAME_MAX_LENGTH = 120;
        string nameMessage =
            string.Format(CategoryValidationMessagesResource.NAME_INVALID, NAME_MAX_LENGTH);

        RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(NAME_MAX_LENGTH)
                .WithMessage(nameMessage);

        const int DESCRIPTION_MAX_LENGTH = 255;
        string descriptionMessage =
            string.Format(CategoryValidationMessagesResource.DESCRIPTION_INVALID, DESCRIPTION_MAX_LENGTH);

        RuleFor(c => c.Description)
                .NotEmpty()
                .MaximumLength(DESCRIPTION_MAX_LENGTH)
                .WithMessage(descriptionMessage);
    }
}
