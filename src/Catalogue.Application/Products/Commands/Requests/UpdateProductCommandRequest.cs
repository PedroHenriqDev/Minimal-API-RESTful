using Catalogue.Application.Abstractions;
using Catalogue.Application.Products.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Products.Commands.Requests;

public sealed class UpdateProductCommandRequest : ProductBase, IRequest<UpdateProductCommandResponse>
{
    [JsonIgnore]
    public Guid Id { get; set; }
}
