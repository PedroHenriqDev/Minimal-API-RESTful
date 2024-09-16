using Catalogue.Application.Users.Commands.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Users.Commands.Requests;

public sealed class UpdateUserCommandRequest :  IRequest<UpdateUserCommandResponse>
{
    [JsonIgnore]
    //Current Name
    public string Name { get; set; } = string.Empty;

    [Required]
    public string NameNew { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }
    public DateTime BirthDate {  get; set; }
}
