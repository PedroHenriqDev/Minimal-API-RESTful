using Catalogue.API.OpenApi;
using Catalogue.API.Filters;
using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Extensions;
using Catalogue.Application.Interfaces.Services;
using Catalogue.Application.Users.Commands.Requests;
using Catalogue.Application.Users.Commands.Responses;
using Catalogue.Application.Users.Queries.Requests;
using Catalogue.Application.Users.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catalogue.API.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        #region Post
        endpoints
        .MapPost("auth/register", RegisterAsync)
        .Produces<RegisterUserCommandResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .WithRegisterDoc();

        endpoints
        .MapPost("auth/login", LoginAsync)
        .Produces<LoginQueryResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithLoginDoc();

        #endregion

        #region Put

        endpoints
        .MapPut("auth/role/{id:guid}", UpdateRoleAsync)
        .AddEndpointFilter<InjectIdFilter>()
        .Produces<UpdateUserRoleCommandRequest>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .WithPutRoleDoc();

        endpoints
        .MapPut("auth/update-user", UpdateAsync)
        .AddEndpointFilter<InjectNameFilter>()
        .Produces<UpdateUserRoleCommandRequest>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .RequireAuthorization()
        .WithPutUserDoc();
        #endregion   
    }

    private static async Task<IResult> RegisterAsync
    (
        [FromBody] RegisterUserCommandRequest request,
        [FromServices] IMediator mediator
    )
    {
        RegisterUserCommandResponse response = await mediator.Send(request);
        return Results.Created(response.Id.ToString(), response);
    }

    private static async Task<IResult> LoginAsync
    (
        [FromBody] LoginQueryRequest request,
        [FromServices] IMediator mediator,
        [FromServices] ITokenService tokenService,
        [FromServices] IClaimService claimService,
        [FromServices] IConfiguration configuration
    )
    {
        LoginQueryResponse response = await mediator.Send(request);

        if (!response.Success)
        {
            return Results.Unauthorized();
        }

        var authClaims = claimService.CreateAuthClaims(response.User!);
        authClaims.AddRole(response.User.RoleName);
        response.Token = tokenService.GenerateToken(authClaims, configuration);

        return Results.Ok(response);
    }

    [Authorize(Policy = "AdminOnly")]
    private static async Task<IResult> UpdateRoleAsync
    (
        [FromRoute] Guid id,
        [FromBody] UpdateUserRoleCommandRequest request,
        [FromServices] IMediator mediator
    )
    {
        UpdateUserRoleCommandResponse response = await mediator.Send(request);
        return Results.Ok(response);
    }

    private static async Task<IResult> UpdateAsync
    (
        [FromBody] UpdateUserCommandRequest request,
        [FromServices] IMediator mediator,
        [FromServices] ITokenService tokenService,
        [FromServices] IClaimService claimService,
        [FromServices] IConfiguration configuration
    )
    {
        UpdateUserCommandResponse response = await mediator.Send(request);

        List<Claim> authClaims = claimService.CreateAuthClaims(response.User!);
        response.NewToken = tokenService.GenerateToken(authClaims, configuration);

        return Results.Ok(response);
    }
}
