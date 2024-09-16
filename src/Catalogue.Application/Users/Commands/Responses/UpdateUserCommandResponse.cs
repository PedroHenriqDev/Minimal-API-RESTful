using Catalogue.Application.DTOs.Responses;

namespace Catalogue.Application.Users.Commands.Responses;

public sealed class UpdateUserCommandResponse
{
    public string WarnMessage => "User information updated successfully. Please use this new token to authentication.";
    public UserResponse User { get; set; } = new();
    public string? NewToken { get; set; }
}
