using Catalogue.Application.Categories.Abstractions.Commands;

namespace Catalogue.Application.Categories.Commands.Responses;

public class UpdateCategoryCommandResponse : CategoryCommandBase
{
    public int Id { get; set; }
}
