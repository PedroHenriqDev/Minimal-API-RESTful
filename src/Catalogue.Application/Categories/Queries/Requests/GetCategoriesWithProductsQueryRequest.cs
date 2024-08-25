using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.Pagination.Parameters;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Requests;

public class GetCategoriesWithProductsQueryRequest : IRequest<GetCategoriesWithProductsQueryResponse>
{
    public QueryParameters? Parameters { get; set; }

    public GetCategoriesWithProductsQueryRequest(QueryParameters parameters)
    {
        Parameters = parameters;
    }
}
