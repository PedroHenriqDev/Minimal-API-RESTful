using Catalogue.Application.Products.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Products.Commands.Requests;

public class DeleteProductCommandRequest : IRequest<DeleteProductCommandResponse>
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public DeleteProductCommandRequest(Guid id)
    {
        Id = id;
    }
}
