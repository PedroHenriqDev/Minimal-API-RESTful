using Catalogue.Application.Users.Commands.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Catalogue.Application.Users.Commands.Requests;

public sealed class UpdateUserCommandRequest :  IRequest<UpdateUserCommandResponse>
{
    [JsonIgnore]
    //Current Name
    public string? Name { get; set; }
    public string? NameNew {  get; set; }
    public string? Email { get; set; }
    public DateTime? BirthDate {  get; set; }
}
