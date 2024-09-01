using Catalogue.Application.Abstractions;

namespace Catalogue.Application.Users.Commands.Responses;

public class RegisterUserCommandResponse : UserBase
{
    public Guid Id { get; set; }
}
