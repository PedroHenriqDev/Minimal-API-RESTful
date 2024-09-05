using Catalogue.Application.Abstractions;

namespace Catalogue.Application.DTOs.Responses;

public sealed class UserResponse : UserBase
{
    public Guid Id {  get; set; }
    public string? RoleName => Role?.ToString();
}
