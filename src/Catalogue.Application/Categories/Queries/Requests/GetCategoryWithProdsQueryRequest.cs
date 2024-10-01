using Catalogue.Application.Abstractions.Requests;
using Catalogue.Application.Categories.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Requests;

public sealed class GetCategoryWithProdsQueryRequest :
    GetCategoryQueryRequestBase, IRequest<GetCategoryWithProdsQueryResponse>
{
    public GetCategoryWithProdsQueryRequest(Guid id) : base(id)
    {
    }
}
