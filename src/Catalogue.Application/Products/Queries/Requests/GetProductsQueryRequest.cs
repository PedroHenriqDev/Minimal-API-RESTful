using Catalogue.Application.Pagination.Parameters;
using Catalogue.Application.Products.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Products.Queries.Requests;

public sealed class GetProductsQueryRequest : IRequest<GetProductsQueryResponse>
{
    public QueryParameters? Parameters { get; set; }
    
    public GetProductsQueryRequest(QueryParameters parameters)
    {
        Parameters = parameters;
    }
}
