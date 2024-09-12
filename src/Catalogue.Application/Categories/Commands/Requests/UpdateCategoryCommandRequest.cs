using Catalogue.Application.Abstractions;
using Catalogue.Application.Categories.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Categories.Commands.Requests;

public sealed class UpdateCategoryCommandRequest : CategoryBase, IRequest<UpdateCategoryCommandResponse>
{
    [JsonIgnore]
    public Guid Id { get; set; }
}
