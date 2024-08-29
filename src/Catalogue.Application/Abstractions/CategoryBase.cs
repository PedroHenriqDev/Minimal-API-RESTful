namespace Catalogue.Application.Abstractions;

public abstract class CategoryBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
