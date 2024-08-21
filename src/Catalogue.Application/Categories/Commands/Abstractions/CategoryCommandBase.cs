using System.Text.Json.Serialization;

namespace Catalogue.Application.Categories.Commands.Abstractions;

public abstract class CategoryCommandBase
{
    public string? Description { get; set; }
    public string? Name { get; set; }
}
