using Catalogue.Application.Abstractions;

namespace Catalogue.Application.Categories.Commands.Responses;

public sealed class CreateCategoryCommandResponse : CategoryBase
{
    public Guid Id {  get; set; }
}
