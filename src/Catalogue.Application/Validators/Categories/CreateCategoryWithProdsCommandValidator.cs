using Catalogue.Application.Abstractions.Validators;
using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Validators.Products;
using FluentValidation;

namespace Catalogue.Application.Validators.Categories;

public class CreateCategoryWithProdsCommandValidator : CategoryBaseValidator<CreateCategoryWithProdsCommandRequest>
{
    public CreateCategoryWithProdsCommandValidator()
    {
        RuleFor(c => c.Products).ForEach(p =>
        {
            p.SetValidator(new ProductRequestValidator());
        });
    }
}
