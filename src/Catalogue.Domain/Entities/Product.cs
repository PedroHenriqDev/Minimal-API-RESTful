using Catalogue.Domain.Validation;
using System.Text.Json.Serialization;

namespace Catalogue.Domain.Entities;

public sealed class Product : Entity
{
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public Decimal? Price { get; set; }

    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }

    public Product()
    {}

    public Product(int id, string name, string description, string imageUrl, DateTime createdAt)
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
            (Id <= 0),
            ValidationMessagesResource.ID_INVALID);

        DomainValidation.When((CreatedAt >= DateTime.UtcNow),
            ValidationMessagesResource.CREATED_AT_INVALID);
    }
}
