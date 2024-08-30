using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Domain.Entities;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Requests;

public class GetCategoryWithProdsQueryRequest : IRequest<GetCategoryWithProdsQueryResponse>
{
    public int Id { get; set; }

    public GetCategoryWithProdsQueryRequest(int id)
    {
        Id = id;
    }
}
