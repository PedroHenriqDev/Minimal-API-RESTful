using Catalogue.Application.DTOs.Responses;

namespace Catalogue.Application.Users.Commands.Responses;

public sealed class UpdateUserCommandResponse
{
    public string WarnMessage => "User information updated successfully. Please log in again.";
    public UserResponse? User { get; set; }
}
