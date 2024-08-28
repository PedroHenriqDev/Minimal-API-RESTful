using Catalogue.Application.Abstractions.Commands;
using Catalogue.Application.Products.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Products.Commands.Requests;

public class UpdateProductCommandRequest : ProductCommandBase, IRequest<UpdateProductCommandResponse>
{
    [JsonIgnore]
    public int Id { get; set; }

}
