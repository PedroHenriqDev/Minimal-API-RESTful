using Catalogue.Application.Products.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Products.Queries.Requests;

public sealed class GetProductWithCatQueryRequest : IRequest<GetProductWithCatQueryResponse>
{
    public Guid Id { get; set; }

    public GetProductWithCatQueryRequest(Guid id)
        => Id = id;
}
