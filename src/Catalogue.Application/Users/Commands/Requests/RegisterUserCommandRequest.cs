using Catalogue.Application.Users.Commands.Responses;
using MediatR;

namespace Catalogue.Application.Users.Commands.Requests;

public class RegisterUserCommandRequest : IRequest<RegisterUserCommandResponse>
{
    public string? Name { get; set; }
    public string? Password { get; set; }
}
