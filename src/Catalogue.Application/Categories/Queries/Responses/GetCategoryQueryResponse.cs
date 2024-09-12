namespace Catalogue.Application.Categories.Queries.Responses;

public class GetCategoryQueryResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Description { get; set; }
}
