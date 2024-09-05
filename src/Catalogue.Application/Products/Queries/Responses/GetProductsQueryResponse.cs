using Catalogue.Application.Interfaces;

namespace Catalogue.Application.Products.Queries.Responses;
 
public sealed class GetProductsQueryResponse
{
    public IPagedList<GetProductQueryResponse>? ProductsPaged { get; set; }

    public GetProductsQueryResponse(IPagedList<GetProductQueryResponse> productsPaged)
    {
        ProductsPaged = productsPaged;
    }
}
