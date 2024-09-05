using Catalogue.Application.Abstractions;

namespace Catalogue.Application.DTOs.Responses;

public sealed class CategoryResponse : CategoryBase
{
    public DateTime CreatedAt { get; set; }
}
