using Catalogue.Application.Abstractions.Commands;

namespace Catalogue.Application.Products.Commands.Responses;

public class UpdateProductCommandResponse : ProductCommandBase
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
}
