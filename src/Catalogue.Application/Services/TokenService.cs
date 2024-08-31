using Catalogue.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Catalogue.Application.Services;

public class TokenService : ITokenService
{
    private readonly ILogger<TokenService> _logger;

    public JwtSecurityTokenHandler TokenHandler { get; set; }

    public TokenService(ILogger<TokenService> logger)
    {
        _logger = logger;
        TokenHandler = new JwtSecurityTokenHandler();
    }

    public string GenerateToken(IEnumerable<Claim> authClaims, IConfiguration configuration)
    {
        byte[] secretKey = GetSecretKey(configuration);

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                                                        SecurityAlgorithms.HmacSha256Signature);

        int.TryParse(configuration["Jwt:ExpireMinutes"], out int expires);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.UtcNow.AddMinutes(expires),
            SigningCredentials = signingCredentials,
        };

        JwtSecurityToken token = TokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return TokenHandler.WriteToken(token);
    }

    private byte[] GetSecretKey(IConfiguration configuration) 
    {
        var secretKey = configuration
            .GetSection("Jwt")
            .GetValue<string>("Secret");

        if(secretKey == null) 
        {
            _logger.LogError("Secret null");
            throw new ArgumentNullException(nameof(secretKey));
        }

        return Encoding.ASCII.GetBytes(secretKey);
    }
}
