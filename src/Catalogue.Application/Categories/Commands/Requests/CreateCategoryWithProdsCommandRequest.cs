using Catalogue.Application.Abstractions;
using Catalogue.Application.Categories.Commands.Responses;
using Catalogue.Application.DTOs.Requests;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Categories.Commands.Requests;

public class CreateCategoryWithProdsCommandRequest
    : CategoryBase, IRequest<CreateCategoryWithProdsCommandResponse>
{
    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ProductRequest>? Products { get; set; }
}
