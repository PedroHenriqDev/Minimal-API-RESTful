using Catalogue.Application.Categories.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Categories.Commands.Requests;

public class DeleteCategoryCommandRequest : IRequest<DeleteCategoryCommandResponse>
{
    [JsonIgnore]
    public int Id { get; set; }
}
