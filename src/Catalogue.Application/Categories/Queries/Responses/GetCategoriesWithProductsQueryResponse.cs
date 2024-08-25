using Catalogue.Application.Interfaces;

namespace Catalogue.Application.Categories.Queries.Responses;

public class GetCategoriesWithProductsQueryResponse
{
    public IPagedList<GetCategoryWithProductsQueryResponse>? CategoriesPaged { get; set; }

    public GetCategoriesWithProductsQueryResponse(IPagedList<GetCategoryWithProductsQueryResponse> categoriesPaged)
    {
        CategoriesPaged = categoriesPaged;
    }
}
