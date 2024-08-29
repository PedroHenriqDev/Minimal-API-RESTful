using Catalogue.Application.Abstractions;
using Catalogue.Application.Categories.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Categories.Commands.Requests;

public sealed class CreateCategoryCommandRequest : CategoryBase, IRequest<CreateCategoryCommandResponse>
{
    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
