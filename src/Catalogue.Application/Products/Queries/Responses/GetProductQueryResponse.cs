namespace Catalogue.Application.Products.Queries.Responses;

public class GetProductQueryResponse
{
    public int Id {  get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Price { get; set; }
}
