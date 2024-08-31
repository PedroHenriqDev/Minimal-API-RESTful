using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Catalogue.Application.Interfaces.Services;

public interface ITokenService
{
    JwtSecurityTokenHandler TokenHandler {get; set; }

    public string GenerateToken(IEnumerable<Claim> claims, IConfiguration configuration);
}
