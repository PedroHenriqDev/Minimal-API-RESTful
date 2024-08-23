namespace Catalogue.Application.Categories.Abstractions.Commands;

public abstract class CategoryCommandBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
