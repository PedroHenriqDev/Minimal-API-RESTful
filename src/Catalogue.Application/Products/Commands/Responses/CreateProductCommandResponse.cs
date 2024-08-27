using Catalogue.Application.Abstractions.Commands;

namespace Catalogue.Application.Products.Commands.Responses;

public sealed class CreateProductCommandResponse : ProductCommandBase
{
    public DateTime CreatedAt { get; set; }
}
