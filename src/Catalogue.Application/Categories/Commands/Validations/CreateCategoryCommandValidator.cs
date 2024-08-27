using Catalogue.Application.Categories.Commands.Requests;

using Catalogue.Application.Resources;
using FluentValidation;

namespace Catalogue.Application.Categories.Commands.Validations;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommandRequest>
{
    public CreateCategoryCommandValidator()
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
