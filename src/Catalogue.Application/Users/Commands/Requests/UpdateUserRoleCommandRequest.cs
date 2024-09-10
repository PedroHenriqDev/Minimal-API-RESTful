using Catalogue.Application.Users.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Users.Commands.Requests;

public sealed class UpdateUserRoleCommandRequest : IRequest<UpdateUserRoleCommandResponse>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? RoleName {  get; set; }
}
