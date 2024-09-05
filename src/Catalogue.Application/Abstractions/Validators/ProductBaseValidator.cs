using Catalogue.Application.Resources;
using Catalogue.Application.Settings;
using FluentValidation;

namespace Catalogue.Application.Abstractions.Validators;

public abstract class ProductBaseValidator<T> : AbstractValidator<T> where T : ProductBase
{
    protected ProductBaseValidator()
    {
        string nameMessage = string.Format(ProductValidationMessagesResource.NAME_INVALID, ProductSettings.MaxName);

        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(ProductSettings.MaxName)
            .WithMessage(nameMessage);

        string descriptionMessage = string.Format(ProductValidationMessagesResource.DESCRIPTION_INVALID, ProductSettings.MaxDescription);

        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(ProductSettings.MaxDescription)
            .WithMessage(descriptionMessage);

        string priceMessage = string.Format(ProductValidationMessagesResource.PRICE_INVALID, ProductSettings.MinPrice, ProductSettings.MaxPrice);

        RuleFor(p => p.Price)
            .GreaterThan(ProductSettings.MinPrice)
            .LessThan(ProductSettings.MaxPrice)
            .WithMessage(priceMessage);
    }
}
