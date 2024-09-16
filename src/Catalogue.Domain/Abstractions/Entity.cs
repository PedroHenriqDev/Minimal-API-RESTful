namespace Catalogue.Domain.Abstractions;

public abstract class Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

