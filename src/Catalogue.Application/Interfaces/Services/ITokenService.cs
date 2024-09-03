using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Catalogue.Application.Interfaces.Services;

public interface ITokenService
{
    public string GenerateToken(IEnumerable<Claim> claims, IConfiguration configuration);
}
