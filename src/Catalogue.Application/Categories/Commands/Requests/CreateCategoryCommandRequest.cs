using Catalogue.Application.Categories.Abstractions.Commands;
using Catalogue.Application.Categories.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Categories.Commands.Requests;

public class CreateCategoryCommandRequest : CategoryCommandBase, IRequest<CreateCategoryCommandResponse>
{
    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
