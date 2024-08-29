using Catalogue.Application.Abstractions;
using Catalogue.Application.Products.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Products.Commands.Requests;

public class UpdateProductCommandRequest : ProductBase, IRequest<UpdateProductCommandResponse>
{
    [JsonIgnore]
    public int Id { get; set; }
}
