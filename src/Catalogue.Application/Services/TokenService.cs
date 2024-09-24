using Catalogue.Application.Extensions;
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

    /// <summary>
    /// This method is responsible for generating a token based on the user's authentication
    /// claims.
    /// </summary>
    /// <param name="authClaims">A list of claims representing the user's authentication information.</param>
    /// <param name="configuration">The configuration settings used to generate the token, including
    /// signing keys and expiration details.</param>
    /// <returns> A JWT token as a string, which is generated and ready to be used in authentication
    /// header.</returns>
    public string GenerateToken(IEnumerable<Claim> authClaims, IConfiguration configuration)
    {
        byte[] secretKey = GetSecretKey(configuration);

        if (authClaims == null || !authClaims.Any()) 
        {
            _logger.LogAndThrow("Claims null", nameof(authClaims));
        }

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

    /// <summary>
    /// Retrieves the secret key from the configuration settings to be used for creating symmetric security key.
    /// </summary>
    /// <param name="configuration">The configuration settings from which the secret key is obtained.</param>
    /// <returns>A byte array representing the secret key, used to instantiate the class 'SymmetricSecurityKey'.
    /// <see cref="SymmetricSecurityKey"/></returns>
    /// <exception cref="ArgumentNullException">Thrown when the secret retrieved from configuration is null.</exception>
    private byte[] GetSecretKey(IConfiguration configuration) 
    {
        var secretKey = configuration
            .GetSection("Jwt")
            .GetValue<string>("Secret");

        if(string.IsNullOrEmpty(secretKey)) 
        {
            _logger.LogAndThrow("Secret Null", nameof(secretKey));
        }

        return Encoding.ASCII.GetBytes(secretKey);
    }
}
