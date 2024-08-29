using Catalogue.Application.Abstractions;

namespace Catalogue.Application.Products.Commands.Responses;

public sealed class CreateProductCommandResponse : ProductBase
{
    public DateTime CreatedAt { get; set; }
}
