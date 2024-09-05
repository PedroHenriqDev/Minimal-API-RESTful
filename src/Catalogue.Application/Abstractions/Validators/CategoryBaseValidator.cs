using Catalogue.Application.Resources;
using Catalogue.Application.Settings;
using FluentValidation;

namespace Catalogue.Application.Abstractions.Validators;

public class CategoryBaseValidator<T> : AbstractValidator<T> where T : CategoryBase
{
    public CategoryBaseValidator()
    {
        string nameMessage =
            string.Format(CategoryValidationMessagesResource.NAME_INVALID, CategorySettings.NameMax);

        RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(CategorySettings.NameMax)
                .WithMessage(nameMessage);

        string descriptionMessage =
            string.Format(CategoryValidationMessagesResource.DESCRIPTION_INVALID, CategorySettings.DescriptionMax);

        RuleFor(c => c.Description)
                .NotEmpty()
                .MaximumLength(CategorySettings.DescriptionMax)
                .WithMessage(descriptionMessage);
    }
}
