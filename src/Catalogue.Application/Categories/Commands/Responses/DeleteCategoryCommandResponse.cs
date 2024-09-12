using Catalogue.Application.Abstractions;

namespace Catalogue.Application.Categories.Commands.Responses;

public sealed class DeleteCategoryCommandResponse : CategoryBase
{
    public Guid Id { get; set; }
}
