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
        .MapPost("auth/register", Register)
        .Produces<RegisterUserCommandResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .WithRegisterDoc();

        endpoints
        .MapPost("auth/login", Login)
        .Produces<LoginQueryResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithLoginDoc();

        #endregion

        #region Put

        endpoints
        .MapPut("auth/role/{id:guid}", UpdateRole)
        .AddEndpointFilter<InjectIdFilter>()
        .Produces<UpdateUserRoleCommandRequest>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .WithPutRoleDoc();

        endpoints
        .MapPut("auth/update-user", UpdateUser)
        .AddEndpointFilter<InjectNameFilter>()
        .Produces<UpdateUserRoleCommandRequest>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .RequireAuthorization()
        .WithPutUserDoc();
        #endregion   
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">The <see cref="RegisterUserCommandRequest"/> object
    /// containing the user's registration data.</param>
    /// <param name="mediator">The <see cref="IMediator"/> service used to send the
    /// registration request to its handler.</param>
    /// <returns>
    /// Returns a <see cref="RegisterUserCommandResponse"/> containing the details of
    /// the newly registered user, 
    /// or an appropriate error response in case of a bad request.
    /// </returns>
    private static async Task<IResult> Register
    (
        [FromBody] RegisterUserCommandRequest request,
        [FromServices] IMediator mediator
    )
    {
        RegisterUserCommandResponse response = await mediator.Send(request);
        return Results.Created(response.Id.ToString(), response);
    }

    /// <summary>
    /// Authenticates a user and generates a JWT token if the login is successful.
    /// </summary>
    /// <param name="request">The <see cref="LoginQueryRequest"/>
    /// object containing the user's login credentials.</param>
    /// <param name="mediator">The <see cref="IMediator"/>
    /// service used to send the login request to the appropriate handler.</param>
    /// <param name="tokenService">The <see cref="ITokenService"/>
    /// service responsible for generating the JWT token.</param>
    /// <param name="claimService">The <see cref="IClaimService"/>
    /// service used to create authentication claims, including user roles.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/>
    /// service used to access configuration settings, such as the JWT secret key.</param>
    /// <returns>
    /// Returns a <see cref="LoginQueryResponse"/> 
    /// containing the authentication details, including the JWT token,
    /// or a 401 Unauthorized response if the login credentials are invalid.
    /// </returns>     
    private static async Task<IResult> Login
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

    /// <summary>
    /// Updates the role of a user specified by their ID. This endpoint is restricted
    /// to users with the "AdminOnly" policy.
    /// </summary>
    /// <param name="id">The unique identifier of the user whose role is to be updated.</param>
    /// <param name="request">The request body containing the new role information.</param>
    /// <param name="mediator">The mediator to handle the command for updating the user role.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the update operation.</returns>
    /// <response code="200">Returns the updated role information.</response>
    /// <response code="404">If the user is not found.</response>
    /// <remarks>
    /// Note: This endpoint requires the caller to be authenticated and authorized with the "AdminOnly"
    /// policy. It will only function if the authenticated user has administrative privileges.
    /// </remarks>
    [Authorize(Policy = "AdminOnly")]
    private static async Task<IResult> UpdateRole
    (
        [FromRoute] Guid id,
        [FromBody] UpdateUserRoleCommandRequest request,
        [FromServices] IMediator mediator
    )
    {
        UpdateUserRoleCommandResponse response = await mediator.Send(request);
        return Results.Ok(response);
    }


    /// <summary>
    /// Updates user's informations.
    /// <summary>
    /// <param name="request">Object containing the user's update data.</param>
    /// <param name="mediator">The MediatR service to process the user update command.</param>
    /// <param name="tokenService">The service responsible for generating JWT tokens.</param>
    /// <param name="claimService">The service responsible for creating authentication claims.</param>
    /// <param name="configuration">Application configuration used to handle tokens.</param>
    /// <returns>Returns the updated user's information and a new JWT token upon success.</returns>
    /// <remarks>
    /// A custom endpoint filter, <see cref="InjectNameFilter"/>, is used to inject the current 
    /// authenticated user's name into the request object before it's processed.
    /// </remarks>
    private static async Task<IResult> UpdateUser
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
