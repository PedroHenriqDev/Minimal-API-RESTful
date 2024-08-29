namespace Catalogue.Application.Abstractions.DTOs;

public class ProductBase
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Price { get; set; }
}
