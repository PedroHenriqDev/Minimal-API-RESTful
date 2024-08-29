using Catalogue.Application.Abstractions;
using Catalogue.Application.DTOs.Responses;

namespace Catalogue.Application.Categories.Commands.Responses;

public class CreateCategoryWithProdsCommandResponse : CategoryBase
{
    public DateTime CreatedAt { get; set; }
    public ICollection<ProductResponse>? Products { get; set; }
}
