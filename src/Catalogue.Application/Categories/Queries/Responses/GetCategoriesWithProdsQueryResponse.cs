using Catalogue.Application.Interfaces;

namespace Catalogue.Application.Categories.Queries.Responses;

public class GetCategoriesWithProdsQueryResponse
{
    public IPagedList<GetCategoryWithProdsQueryResponse>? CategoriesPaged { get; set; }

    public GetCategoriesWithProdsQueryResponse(IPagedList<GetCategoryWithProdsQueryResponse> categoriesPaged)
    {
        CategoriesPaged = categoriesPaged;
    }
}
