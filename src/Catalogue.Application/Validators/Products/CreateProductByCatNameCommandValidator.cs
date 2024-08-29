using Catalogue.Application.Abstractions.Validators;
using Catalogue.Application.Products.Commands.Requests;
using Catalogue.Application.Resources;
using FluentValidation;

namespace Catalogue.Application.Validators.Products;

public class CreateProductByCatNameCommandValidator : ProductBaseValidator<CreateProductByCatNameCommandRequest>
{
    public CreateProductByCatNameCommandValidator()
    {
        RuleFor(p => p.CategoryName)
            .NotEmpty()
            .NotNull()
            .WithMessage(ProductValidationMessagesResource.CATEGORY_NAME_INVALID);
    }
}
