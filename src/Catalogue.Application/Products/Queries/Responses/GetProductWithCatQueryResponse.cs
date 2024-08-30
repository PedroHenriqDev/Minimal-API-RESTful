using Catalogue.Application.Abstractions;
using Catalogue.Application.DTOs.Responses;

namespace Catalogue.Application.Products.Queries.Responses;

public class GetProductWithCatQueryResponse : ProductBase
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public CategoryResponse? Category { get; set; }
}
