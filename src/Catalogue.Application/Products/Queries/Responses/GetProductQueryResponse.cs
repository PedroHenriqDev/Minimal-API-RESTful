using Catalogue.Application.Abstractions;

namespace Catalogue.Application.Products.Queries.Responses;

public class GetProductQueryResponse : ProductBase
{
    public int Id {  get; set; }
}
