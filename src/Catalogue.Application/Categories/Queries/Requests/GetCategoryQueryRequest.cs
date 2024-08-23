using Catalogue.Application.Categories.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Requests;

public class GetCategoryQueryRequest : IRequest<GetCategoryQueryResponse>
{
    public int Id { get; set; }

    public GetCategoryQueryRequest(int id)
    {
        Id = id;
    }
}
