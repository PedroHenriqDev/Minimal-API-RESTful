using Catalogue.Application.Categories.Commands.Abstractions;

namespace Catalogue.Application.Categories.Commands.Responses;

public class UpdateCategoryCommandResponse : CategoryCommandBase
{
    int Id { get; set; }
}
