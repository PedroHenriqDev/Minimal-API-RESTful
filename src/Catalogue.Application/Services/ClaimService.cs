using Catalogue.Application.Interfaces.Services;
using Catalogue.Domain.Entities;
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

    public void AddRolesToClaims(IEnumerable<string> roles, List<Claim> claims)
    {
        if (roles == null)
        {
            _logger.LogError("Roles null");
            throw new ArgumentNullException(nameof(roles));
        }

        if (claims == null)
        {
            _logger.LogError("Claims null");
            throw new ArgumentNullException(nameof(claims));
        }

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
    }

    public List<Claim> CreateAuthClaims(User user)
    {
        if(user == null) 
        {
            _logger.LogError("User null");
            throw new ArgumentNullException(nameof(user));
        }

        return new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
    }
}
