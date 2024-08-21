using System.Text.Json.Serialization;

namespace Catalogue.Application.Categories.Commands.Abstractions;

public abstract class CategoryCommandBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
