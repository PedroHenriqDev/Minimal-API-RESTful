using Catalogue.Domain.Enums;

namespace Catalogue.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }   
    public string? Password { get; set; }
    public DateTime BirthDate { get; set; }
    public Role Role { get; set; }

    public void AssignNewId() 
    {
        Id = Guid.NewGuid();
    }
}
