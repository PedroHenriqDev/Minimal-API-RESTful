using Catalogue.Application.Users.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Users.Queries.Requests;

public sealed class LoginQueryRequest : IRequest<LoginQueryResponse>
{
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
