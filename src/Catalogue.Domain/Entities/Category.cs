using Catalogue.Domain.Validation;

namespace Catalogue.Domain.Entities;

public sealed class Category : Entity
{
    public string? Description { get; set; }

    public ICollection<Product>? Products { get; set; }

    public Category()
    {}

    public Category(Guid id, string name, string description, DateTime createdAt) 
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
     
        ValidateDomain();
    }

    public Category(string name, string description, DateTime createdAt) 
    {
        Name = name;
        Description = description;
        CreatedAt = createdAt;
    
        ValidateDomain();
    }

    private void ValidateDomain()
    {
        DomainValidation.When(
            (Name is null || string.IsNullOrWhiteSpace(Name)),
            ValidationMessagesResource.NAME_INVALID);            

        DomainValidation.When(
            (Description is null || string.IsNullOrWhiteSpace(Description)),
            ValidationMessagesResource.DESCRIPTION_INVALID);

        DomainValidation.When(
            (CreatedAt >= DateTime.UtcNow),
            ValidationMessagesResource.CREATED_AT_INVALID);
    }
}

