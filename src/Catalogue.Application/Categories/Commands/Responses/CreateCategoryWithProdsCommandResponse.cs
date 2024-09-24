using Catalogue.Application.Abstractions;
using Catalogue.Application.DTOs.Responses;

namespace Catalogue.Application.Categories.Commands.Responses;

public sealed class CreateCategoryWithProdsCommandResponse : CategoryBase
{
    public Guid Id {get; set;}
    public DateTime CreatedAt { get; set; }
    public ICollection<ProductResponse>? Products { get; set; }
}
