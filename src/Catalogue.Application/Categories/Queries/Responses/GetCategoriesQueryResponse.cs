using Catalogue.Application.Interfaces;

namespace Catalogue.Application.Categories.Queries.Responses;

public sealed class GetCategoriesQueryResponse
{
    public IPagedList<GetCategoryQueryResponse>? CategoriesPaged { get; set; }

    public GetCategoriesQueryResponse(IPagedList<GetCategoryQueryResponse> categoriesPaged)
        => CategoriesPaged = categoriesPaged;
}
