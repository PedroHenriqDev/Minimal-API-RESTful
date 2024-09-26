using System.Text.Json.Serialization;
using Catalogue.Application.Interfaces;

namespace Catalogue.Application.Products.Queries.Responses;

public sealed class GetProductsWithCatQueryResponse
{
    public IPagedList<GetProductWithCatQueryResponse>? ProductsPaged { get; set; }

    [JsonConstructor]
    public GetProductsWithCatQueryResponse(IPagedList<GetProductWithCatQueryResponse> productsPaged)
        => ProductsPaged = productsPaged;
    
}
