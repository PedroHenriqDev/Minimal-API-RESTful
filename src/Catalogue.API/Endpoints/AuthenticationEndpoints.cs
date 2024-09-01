using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Users.Commands.Requests;
using Catalogue.Application.Users.Commands.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Endpoints;

public static class AuthenticationEndpoints
{
    private const string authEndpoint = "Authentication";
    
    public static void MapPostAuthEndpoints(this WebApplication app) 
    {
        app.MapPost("register", async ([FromBody] RegisterUserCommandRequest request,
                                       [FromServices] IMediator mediator) =>
        {

            RegisterUserCommandResponse response = await mediator.Send(request);
            return Results.Created(string.Empty, response);

        })
        .Produces<RegisterUserCommandResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest).WithTags(authEndpoint);
    }
}
