using Catalogue.Application.Abstractions;
using Catalogue.Application.Products.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Products.Commands.Requests;

public sealed class CreateProductByCatNameCommandRequest : ProductBase, IRequest<CreateProductCommandResponse>
{
    public string CategoryName { get; set; } = string.Empty;

    [JsonIgnore]
    public Guid CategoryId { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
