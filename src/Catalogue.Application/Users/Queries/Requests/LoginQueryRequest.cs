using Catalogue.Application.Users.Queries.Responses;
using MediatR;

namespace Catalogue.Application.Users.Queries.Requests;

public class LoginQueryRequest : IRequest<LoginQueryResponse>
{
    public string? Name { get; set; }
    public string? Password { get; set; }
}
