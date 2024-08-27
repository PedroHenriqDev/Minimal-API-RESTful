using Catalogue.Application.Abstractions.Commands;

namespace Catalogue.Application.Products.Commands.Responses;

public class DeleteProductCommandResponse : ProductCommandBase
{
    public int Id { get; set; }
    public string? CategoryName { get; set; }
}
