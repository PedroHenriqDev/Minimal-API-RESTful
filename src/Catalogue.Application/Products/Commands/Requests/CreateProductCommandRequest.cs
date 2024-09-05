using Catalogue.Application.Abstractions;
using Catalogue.Application.Products.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Products.Commands.Requests;

public sealed class CreateProductCommandRequest : ProductBase, IRequest<CreateProductCommandResponse>
{
    public int CategoryId { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
