using Catalogue.Application.Products.Queries.Responses;

namespace Catalogue.Application.Categories.Queries.Responses;

public class GetCategoryWithProductsQueryResponse 
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Description { get; set; }
    public ICollection<GetProductQueryResponse> Products = [];
}
