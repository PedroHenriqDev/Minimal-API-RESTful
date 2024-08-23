using Catalogue.Application.Categories.Abstractions.Commands;
using Catalogue.Application.Categories.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Categories.Commands.Requests;

public class UpdateCategoryCommandRequest : CategoryCommandBase, IRequest<UpdateCategoryCommandResponse>
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; }
}
