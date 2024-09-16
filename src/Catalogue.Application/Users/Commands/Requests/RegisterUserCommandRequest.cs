using Catalogue.Application.Users.Commands.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Users.Commands.Requests;

public sealed class RegisterUserCommandRequest : IRequest<RegisterUserCommandResponse>
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;

    [JsonIgnore]
    public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;
}
