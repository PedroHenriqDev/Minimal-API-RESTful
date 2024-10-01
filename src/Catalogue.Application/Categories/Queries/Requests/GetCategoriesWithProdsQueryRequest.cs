using Catalogue.Application.Abstractions.Responses;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.Pagination.Parameters;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Requests;

public sealed class GetCategoriesWithProdsQueryRequest :
    GetCategoriesQueryRequestBase, IRequest<GetCategoriesWithProdsQueryResponse>
{
    public GetCategoriesWithProdsQueryRequest(QueryParameters parameters) : base(parameters)
    {
    }
}
