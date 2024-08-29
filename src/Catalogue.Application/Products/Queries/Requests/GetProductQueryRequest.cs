using Catalogue.Application.Products.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Products.Queries.Requests;

public class GetProductQueryRequest : IRequest<GetProductQueryResponse>
{
    public int Id { get; set; }
    
    public GetProductQueryRequest(int id)
    {
        Id = id;
    }
}
