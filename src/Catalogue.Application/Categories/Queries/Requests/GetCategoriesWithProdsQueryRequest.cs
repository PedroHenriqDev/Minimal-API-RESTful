using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.Pagination.Parameters;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Requests;

public class GetCategoriesWithProdsQueryRequest : IRequest<GetCategoriesWithProdsQueryResponse>
{
    public QueryParameters? Parameters { get; set; }

    public GetCategoriesWithProdsQueryRequest(QueryParameters parameters)
    {
        Parameters = parameters;
    }
}
