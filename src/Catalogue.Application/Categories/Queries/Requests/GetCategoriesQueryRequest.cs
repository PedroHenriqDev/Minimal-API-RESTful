using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.Pagination.Parameters;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Requests;

public class GetCategoriesQueryRequest : IRequest<GetCategoriesQueryResponse>
{
    public QueryParameters? Parameters { get; set; }
    
    public GetCategoriesQueryRequest(QueryParameters parameters)
        => Parameters = parameters;
}
