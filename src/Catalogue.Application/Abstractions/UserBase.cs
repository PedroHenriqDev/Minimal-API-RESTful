using Catalogue.Domain.Enums;

namespace Catalogue.Application.Abstractions;

public abstract class UserBase
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public Role? Role { get; set; }
    public DateTime BirthDate { get; set; }
}
