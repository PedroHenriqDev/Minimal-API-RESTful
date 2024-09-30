using Catalogue.Application.Categories.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Requests;

public class GetCategoryWithProdsQueryRequest : IRequest<GetCategoryWithProdsQueryResponse>
{
    public Guid Id { get; set; }

    public GetCategoryWithProdsQueryRequest(Guid id)
        => Id = id;
    
}
