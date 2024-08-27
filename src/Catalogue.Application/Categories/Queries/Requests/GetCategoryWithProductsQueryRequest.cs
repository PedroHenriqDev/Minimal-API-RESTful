using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Domain.Entities;
using MediatR;

namespace Catalogue.Application.Categories.Queries.Requests;

public class GetCategoryWithProductsQueryRequest : IRequest<GetCategoryWithProductsQueryResponse>
{
    public int Id { get; set; }

    public GetCategoryWithProductsQueryRequest(int id)
    {
        Id = id;
    }
}
