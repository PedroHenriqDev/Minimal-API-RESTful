using Catalogue.Application.Abstractions;
using Catalogue.Application.Extensions;
using Catalogue.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Catalogue.Application.Services;

public class ClaimService : IClaimService
{
    private readonly ILogger<ClaimService> _logger;

    public ClaimService(ILogger<ClaimService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Generates a list of authentication claims based on the provided user.
    /// </summary>
    /// <typeparam name="TUser">A generic type representing a user that inherits from 'UserBase'. 
    /// This ensures that the necessary properties for creating authentication claims are available.</typeparam>
    /// <param name="user">An instance of the user object used to generate the claims.</param>
    /// <returns>A list of claims containing the user's authentication information.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided user object is null.</exception>
    public List<Claim> CreateAuthClaims<TUser>(TUser user) where TUser : UserBase 
    {
        if(user == null) 
        {
            _logger.LogAndThrow("User null", nameof(user));
        }

        return new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
    }
}
