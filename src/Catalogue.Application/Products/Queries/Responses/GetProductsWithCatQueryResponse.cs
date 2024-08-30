using Catalogue.Application.Interfaces;

namespace Catalogue.Application.Products.Queries.Responses;

public class GetProductsWithCatQueryResponse
{
    public IPagedList<GetProductWithCatQueryResponse>? ProductsPaged { get; set; }

    public GetProductsWithCatQueryResponse(IPagedList<GetProductWithCatQueryResponse> productsPaged)
    {
        ProductsPaged = productsPaged;
    }
}
