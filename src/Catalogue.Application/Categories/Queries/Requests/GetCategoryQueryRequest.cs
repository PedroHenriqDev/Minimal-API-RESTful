using Catalogue.Application.Categories.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Requests;

public class GetCategoryQueryRequest : IRequest<GetCategoryQueryResponse>
{
    public Guid Id { get; set; }

    public GetCategoryQueryRequest(Guid id)
        => Id = id;
    
}
