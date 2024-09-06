using Catalogue.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Catalogue.Tests.UnitTests;

public class TokenTests
{
    private readonly TokenService _tokenService;
    private readonly Mock<ILogger<TokenService>> _mockLogger;
    private readonly IConfiguration _configuration;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public TokenTests()
    {
        _mockLogger = new Mock<ILogger<TokenService>>();

        _tokenService = new TokenService(_mockLogger.Object);

        var inMemorySettings = new Dictionary<string, string>
        {
            {"Jwt:Secret", "0cc175b9c0f1b6a831c399e269772661" },
            { "Jwt:ExpireMinutes", "60" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public (List<Claim> authClaims, string userName, string jti) Arrange()
    {
        string userName = "Test_User";
        string jti = Guid.NewGuid().ToString();
        List<Claim> authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(JwtRegisteredClaimNames.Jti, jti)
        };

        return (authClaims, userName, jti);
    }


    /// <summary>
    /// Verifies that the token generation process correctly creates a valid token with the expected claims.
    /// </summary>
    [Fact]
    public void WhenGenerateToken_GivenAuthClaimsAndConfiguration_ReturnWriteTokenValid()
    {
        //Arrange
        (List<Claim> authClaims, string userName, string jti) = Arrange();

        //Act
        string token = _tokenService.GenerateToken(authClaims, _configuration);

        var jwtToken = _tokenHandler.ReadJwtToken(token);

        //Assert
        Assert.NotNull(token);
        Assert.Contains(jwtToken.Claims, claim => claim.Type == "unique_name" && claim.Value == userName);
        Assert.Contains(jwtToken.Claims, claim => claim.Type == JwtRegisteredClaimNames.Jti);
        Assert.True(jwtToken.ValidFrom > DateTime.Now);
    }

    /// <summary>
    /// Verifies that a generated token is valid according to the specified validation parameters.
    /// </summary>
    [Fact]
    public void WhenValidateToken_GivenValidToken_ReturnTrue()
    {
        //Arrange
        byte[] secretKey = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
        (List<Claim> authClaims, string userName, string jti) = Arrange();

        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };

        //Act
        string token = _tokenService.GenerateToken(authClaims, _configuration);

        ClaimsPrincipal principal = _tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

        //Assert 
        Assert.NotNull(principal);
        Assert.IsType<JwtSecurityToken>(validatedToken);
    }
}
