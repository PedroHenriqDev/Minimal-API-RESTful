using Catalogue.Domain.Abstractions;
using Catalogue.Domain.Validation;
using System.Text.Json.Serialization;

namespace Catalogue.Domain.Entities;

public sealed class Product : Entity
{
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } 
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }

    public Product()
    {}

    public Product(Guid id, string name, string description, string imageUrl, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CreatedAt = createdAt;
    }

    public Product(string name, string description, string imageUrl, DateTime createdAt) 
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CreatedAt = createdAt;
    } 

    public void ValidateDomain() 
    {
        DomainValidation.When(
            (Name is null ||
            string.IsNullOrWhiteSpace(Name)),
            ValidationMessagesResource.NAME_INVALID);

        DomainValidation.When(
            (Description is null ||
            string.IsNullOrWhiteSpace(Description)),
            ValidationMessagesResource.DESCRIPTION_INVALID);

        DomainValidation.When(
            (ImageUrl is null ||
            string.IsNullOrWhiteSpace(ImageUrl)),
            ValidationMessagesResource.IMAGE_URL_INVALID);

        DomainValidation.When(
            (Equals(Guid.Empty == Id)),
            ValidationMessagesResource.ID_INVALID);

        DomainValidation.When((CreatedAt >= DateTime.UtcNow),
            ValidationMessagesResource.CREATED_AT_INVALID);
    }
}
