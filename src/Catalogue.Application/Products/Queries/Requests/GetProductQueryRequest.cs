using Catalogue.Application.Products.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Products.Queries.Requests;

public sealed class GetProductQueryRequest : IRequest<GetProductQueryResponse>
{
    public Guid Id { get; set; }
    
    public GetProductQueryRequest(Guid id)
        => Id = id;
}
