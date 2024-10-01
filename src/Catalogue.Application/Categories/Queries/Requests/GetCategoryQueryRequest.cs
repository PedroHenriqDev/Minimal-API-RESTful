using Catalogue.Application.Abstractions.Requests;
using Catalogue.Application.Categories.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Requests;

public sealed class GetCategoryQueryRequest : 
    GetCategoryQueryRequestBase, IRequest<GetCategoryQueryResponse>
{
    public GetCategoryQueryRequest(Guid id) : base (id)
    {
    } 
}
