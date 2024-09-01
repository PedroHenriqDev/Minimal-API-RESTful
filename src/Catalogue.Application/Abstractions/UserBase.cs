namespace Catalogue.Application.Abstractions;

public class UserBase
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public DateTime BirthDate { get; set; }
}
