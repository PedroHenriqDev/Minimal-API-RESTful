using Catalogue.Application.Categories.Abstractions.Commands;

namespace Catalogue.Application.Categories.Commands.Responses;

public sealed class DeleteCategoryCommandResponse : CategoryCommandBase
{
    public int Id { get; set; }
}
