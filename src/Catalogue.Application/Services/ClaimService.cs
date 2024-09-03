using Catalogue.Application.Abstractions;
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

    public void AddRoleToClaims(string role, List<Claim> claims)
    {
        if (string.IsNullOrEmpty(role))
        {
            _logger.LogError("Roles null");
            throw new ArgumentNullException(nameof(role));
        }

        if (string.IsNullOrEmpty(role))
        {
            _logger.LogError("Claims null");
            throw new ArgumentNullException(nameof(claims));
        }

        claims.Add(new Claim(ClaimTypes.Role, role));
    }

    public List<Claim> CreateAuthClaims<TUser>(TUser user) where TUser : UserBase
    {
        if(user == null) 
        {
            _logger.LogError("User null");
            throw new ArgumentNullException(nameof(user));
        }

        return new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
    }
}
