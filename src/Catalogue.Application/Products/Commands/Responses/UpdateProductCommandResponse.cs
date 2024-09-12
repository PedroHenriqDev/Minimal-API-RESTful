using Catalogue.Application.Abstractions;

namespace Catalogue.Application.Products.Commands.Responses;

public sealed class UpdateProductCommandResponse : ProductBase
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
}
