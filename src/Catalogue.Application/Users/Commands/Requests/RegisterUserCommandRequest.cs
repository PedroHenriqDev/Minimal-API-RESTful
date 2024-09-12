using Catalogue.Application.Users.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Users.Commands.Requests;

public sealed class RegisterUserCommandRequest : IRequest<RegisterUserCommandResponse>
{
    public string? Name { get; set; }
    public string? Password { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;
}
