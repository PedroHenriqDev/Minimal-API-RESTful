using Catalogue.Application.DTOs.Responses;

namespace Catalogue.Application.Users.Commands.Responses;

public sealed class UpdateUserRoleCommandResponse
{
    public UserResponse User { get; set; } = new();
}
