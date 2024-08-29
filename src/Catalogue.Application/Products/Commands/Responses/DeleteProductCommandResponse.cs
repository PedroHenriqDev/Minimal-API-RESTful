using Catalogue.Application.Abstractions;

namespace Catalogue.Application.Products.Commands.Responses;

public class DeleteProductCommandResponse : ProductBase
{
    public int Id { get; set; }
    public string? CategoryName { get; set; }
}
