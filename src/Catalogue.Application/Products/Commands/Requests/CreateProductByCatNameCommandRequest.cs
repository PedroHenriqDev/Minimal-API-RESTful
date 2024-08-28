using Catalogue.Application.Abstractions.Commands;
using Catalogue.Application.Products.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Products.Commands.Requests;

public sealed class CreateProductByCatNameCommandRequest : ProductCommandBase, IRequest<CreateProductCommandResponse>
{
    public string CategoryName { get; set; } = string.Empty;

    [JsonIgnore]
    public int CategoryId { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
