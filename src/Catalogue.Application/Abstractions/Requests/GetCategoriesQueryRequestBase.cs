using Catalogue.Application.Pagination.Parameters;

namespace Catalogue.Application.Abstractions.Responses;

public class GetCategoriesQueryRequestBase
{
    public QueryParameters? Parameters { get; set; }
    
    public GetCategoriesQueryRequestBase(QueryParameters parameters)
        => Parameters = parameters;
}