using System.Text.Json.Serialization;
using Catalogue.Application.Interfaces;

namespace Catalogue.Application.Categories.Queries.Responses;

public sealed class GetCategoriesWithProdsQueryResponse
{
    public IPagedList<GetCategoryWithProdsQueryResponse>? CategoriesPaged { get; set; }

    [JsonConstructor]
    public GetCategoriesWithProdsQueryResponse(IPagedList<GetCategoryWithProdsQueryResponse> categoriesPaged)
        => CategoriesPaged = categoriesPaged;
}
