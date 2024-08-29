using Catalogue.Application.Abstractions;

namespace Catalogue.Application.Products.Commands.Responses;

public class UpdateProductCommandResponse : ProductBase
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
}
