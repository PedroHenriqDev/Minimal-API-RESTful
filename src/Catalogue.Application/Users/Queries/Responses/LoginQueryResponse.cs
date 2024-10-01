using Catalogue.Application.DTOs.Responses;

namespace Catalogue.Application.Users.Queries.Responses;

public sealed class LoginQueryResponse
{
    public bool Success { get; set; }
    public UserResponse? User { get; set; }
    public string Token { get; set; } = string.Empty;
}
