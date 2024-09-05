using Catalogue.Application.Pagination.Parameters;
using Catalogue.Application.Products.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Products.Queries.Requests;

public sealed class GetProductsWithCatQueryRequest : IRequest<GetProductsWithCatQueryResponse>
{
    public QueryParameters? Parameters { get; set; }

    public GetProductsWithCatQueryRequest(QueryParameters parameters)
    {
        Parameters = parameters;
    }
}
