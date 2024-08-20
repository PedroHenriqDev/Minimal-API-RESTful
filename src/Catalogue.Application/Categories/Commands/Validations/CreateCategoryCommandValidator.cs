using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Resources;
using FluentValidation;

namespace Catalogue.Application.Categories.Commands.Validations;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommandRequest>
{
    public CreateCategoryCommandValidator() 
    {
        const int nameMaxLength = 120; 
        string nameMessage =
            string.Format(ValidationCategoryMessagesResource.NAME_INVALID, nameMaxLength);

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(nameMaxLength)
            .WithMessage(nameMessage);

        const int descriptionMaxLenght = 255;
        string descriptionMessage =
            string.Format(ValidationCategoryMessagesResource.DESCRIPTION_INVALID, descriptionMaxLenght);

        RuleFor(c => c.Description)
            .MaximumLength(descriptionMaxLenght)
            .WithMessage(descriptionMessage);
    }
}
