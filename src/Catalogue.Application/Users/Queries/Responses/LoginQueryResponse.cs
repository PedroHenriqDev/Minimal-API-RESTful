using Catalogue.Application.DTOs.Responses;

namespace Catalogue.Application.Users.Queries.Responses;

public class LoginQueryResponse
{
    public bool Success { get; set; }
    public UserResponse? User { get; set; }
    public string? Token { get; set; }
}
