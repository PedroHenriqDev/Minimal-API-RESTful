using Catalogue.Application.Categories.Abstractions.Commands;
using Catalogue.Application.DTOs.Responses;

namespace Catalogue.Application.Categories.Commands.Responses;

public class CreateCategoryWithProdsCommandResponse : CategoryCommandBase
{
    public DateTime CreatedAt { get; set; }
    public ICollection<ProductResponse>? Products { get; set; }
}
