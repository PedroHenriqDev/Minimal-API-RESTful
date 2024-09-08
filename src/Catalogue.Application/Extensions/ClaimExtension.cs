using System.Security.Claims;

namespace Catalogue.Application.Extensions;

public static class ClaimExtension
{
    public static void AddRole(this List<Claim> claims, string role)
    {
        if (string.IsNullOrEmpty(role))
        {
            throw new ArgumentNullException(nameof(role));
        }

        if (claims == null || !claims.Any())
        {
            throw new ArgumentNullException(nameof(claims));
        }

        claims.Add(new Claim(ClaimTypes.Role, role));
    }
}
