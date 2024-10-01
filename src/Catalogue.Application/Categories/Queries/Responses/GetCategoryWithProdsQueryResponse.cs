using Catalogue.Application.Abstractions;
using Catalogue.Application.Products.Queries.Responses;

namespace Catalogue.Application.Categories.Queries.Responses;

public sealed class GetCategoryWithProdsQueryResponse : CategoryBase
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<GetProductQueryResponse>? Products { get; set; }
}
