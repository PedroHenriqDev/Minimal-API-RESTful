using Catalogue.API.Extensions;
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
    private const string authEndpoint = "Authentication";

    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        #region Post
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The <see cref="RegisterUserCommandRequest"/> object containing the user's registration data.</param>
        /// <param name="mediator">The <see cref="IMediator"/> service used to send the registration request to its handler.</param>
        /// <returns>
        /// Returns a <see cref="RegisterUserCommandResponse"/> containing the details of the newly registered user, 
        /// or an appropriate error response in case of a bad request.
        /// </returns>
        endpoints.MapPost("auth/register", async ([FromBody] RegisterUserCommandRequest request,
                                                  [FromServices] IMediator mediator) =>
        {
            RegisterUserCommandResponse response = await mediator.Send(request);
            return Results.Created(response.Id.ToString(), response);

        })
        .Produces<RegisterUserCommandResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .WithTags(authEndpoint)
        .WithRegisterDoc();

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
        endpoints.MapPost("auth/login", async ([FromBody] LoginQueryRequest request,
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
            authClaims.AddRole(response.User.RoleName);
            response.Token = tokenService.GenerateToken(authClaims, configuration);

            return Results.Ok(response);
        })
        .Produces<LoginQueryResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithTags(authEndpoint)
        .WithLoginDoc();

        #endregion

        #region Put
        endpoints.MapPut("auth/role/{id:guid}", [Authorize(Policy = "AdminOnly")]
                 async ([FromRoute] Guid id,
                        [FromBody] UpdateUserRoleCommandRequest request,
                        [FromServices] IMediator mediator) =>
        {

            UpdateUserRoleCommandResponse response = await mediator.Send(request);
            return Results.Ok(response);

        })
        .AddEndpointFilter<InjectIdFilter>()
        .Produces<UpdateUserRoleCommandRequest>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .RequireAuthorization();

        endpoints.MapPut("auth/update-user", async ([FromBody] UpdateUserCommandRequest request,
                                                    [FromServices] IMediator mediator,
                                                    [FromServices] ITokenService tokenService,
                                                    [FromServices] IClaimService claimService,
                                                    [FromServices] IConfiguration configuration) =>
        {
            UpdateUserCommandResponse response = await mediator.Send(request);

            List<Claim> authClaims = claimService.CreateAuthClaims(response.User!);
            response.NewToken = tokenService.GenerateToken(authClaims, configuration);

            return Results.Ok(response);

        })
        .AddEndpointFilter<InjectNameFilter>()
        .Produces<UpdateUserRoleCommandRequest>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .RequireAuthorization()
        .WithTags(authEndpoint);
     #endregion   
    }
}
