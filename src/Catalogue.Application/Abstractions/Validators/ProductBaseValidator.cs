using Catalogue.Application.Resources;
using FluentValidation;

namespace Catalogue.Application.Abstractions.Validators;

public abstract class ProductBaseValidator<T> : AbstractValidator<T> where T : ProductBase
{
    protected ProductBaseValidator()
    {
        const int MAX_NAME = 120;
        string nameMessage = string.Format(ProductValidationMessagesResource.NAME_INVALID, MAX_NAME);

        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(MAX_NAME)
            .WithMessage(nameMessage);

        const int MAX_DESCRIPTION = 255;
        string descriptionMessage = string.Format(ProductValidationMessagesResource.DESCRIPTION_INVALID, MAX_DESCRIPTION);

        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(MAX_DESCRIPTION)
            .WithMessage(descriptionMessage);


        const int MAX_PRICE = 99999999;
        const int MIN_PRICE = 0;
        string priceMessage = string.Format(ProductValidationMessagesResource.PRICE_INVALID, MIN_PRICE, MAX_PRICE);

        RuleFor(p => p.Price)
            .GreaterThan(MIN_PRICE)
            .LessThan(MAX_PRICE)
            .WithMessage(priceMessage);
    }
}
