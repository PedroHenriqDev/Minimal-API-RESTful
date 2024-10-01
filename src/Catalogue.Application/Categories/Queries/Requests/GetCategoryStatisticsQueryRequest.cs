using Catalogue.Application.Abstractions.Requests;
using Catalogue.Application.Categories.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Requests;

public sealed class GetCategoryStatisticsQueryRequest :
    GetCategoryQueryRequestBase, IRequest<GetCategoryStatisticsQueryResponse>
{
    public GetCategoryStatisticsQueryRequest(Guid id) : base(id)
    {
    }
}