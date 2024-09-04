using Catalogue.Application.Users.Commands.Responses;
using MediatR;

namespace Catalogue.Application.Users.Commands.Requests;

public class UpdateUserRoleCommandRequest : IRequest<UpdateUserRoleCommandResponse>
{
    public Guid Id { get; set; }
    public string? RoleName {  get; set; }
}
