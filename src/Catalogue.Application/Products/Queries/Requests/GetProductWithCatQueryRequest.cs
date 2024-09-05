using Catalogue.Application.Products.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Products.Queries.Requests;

public sealed class GetProductWithCatQueryRequest : IRequest<GetProductWithCatQueryResponse>
{
    public int Id { get; set; }

    public GetProductWithCatQueryRequest(int id)
    {
        Id = id;
    }
}
