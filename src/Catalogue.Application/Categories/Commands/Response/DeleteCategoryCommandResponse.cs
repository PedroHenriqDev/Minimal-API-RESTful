using Catalogue.Application.Categories.Commands.Abstractions;

namespace Catalogue.Application.Categories.Commands.Response;

public class DeleteCategoryCommandResponse : CategoryCommandBase
{
    public int Id { get; set; }
}
