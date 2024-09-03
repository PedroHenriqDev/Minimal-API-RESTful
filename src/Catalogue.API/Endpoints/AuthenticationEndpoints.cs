using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Interfaces.Services;
using Catalogue.Application.Users.Commands.Requests;
using Catalogue.Application.Users.Commands.Responses;
using Catalogue.Application.Users.Queries.Requests;
using Catalogue.Application.Users.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Endpoints;

public static class AuthenticationEndpoints
{
    private const string authEndpoint = "Authentication";
    
    public static void MapPostAuthEndpoints(this WebApplication app) 
    {
        app.MapPost("auth/register", async ([FromBody] RegisterUserCommandRequest request,
                                            [FromServices] IMediator mediator) =>
        {

            RegisterUserCommandResponse response = await mediator.Send(request);
            return Results.Created(string.Empty, response);

        })
        .Produces<RegisterUserCommandResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .WithTags(authEndpoint);

        app.MapPost("auth/login", async ([FromBody] LoginQueryRequest request,
                                         [FromServices] IMediator mediator,
                                         [FromServices] ITokenService tokenService,
                                         [FromServices] IClaimService claimService,
                                         [FromServices] IConfiguration configuration) =>
        {
            LoginQueryResponse response = await mediator.Send(request);

            if (!response.Success)
            {
                return Results.Unauthorized();
            }

            var authClaims = claimService.CreateAuthClaims(response.User!);
            claimService.AddRoleToClaims(response.User.RoleName, authClaims);
            response.Token = tokenService.GenerateToken(authClaims, configuration);

            return Results.Ok(response);

        }).Produces<LoginQueryResponse>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status401Unauthorized)
          .WithTags(authEndpoint);
    }
}
