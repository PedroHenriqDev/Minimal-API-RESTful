using Catalogue.Application.Abstractions;

namespace Catalogue.Application.Categories.Queries.Responses;

public sealed class GetCategoryQueryResponse : CategoryBase
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
}
