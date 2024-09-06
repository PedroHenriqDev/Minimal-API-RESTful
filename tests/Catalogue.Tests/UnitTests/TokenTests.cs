using Catalogue.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Catalogue.Tests.UnitTests;

public class TokenTests
{
    private readonly TokenService _tokenService;
    private readonly Mock<ILogger<TokenService>> _mockLogger;
    private readonly IConfiguration _configuration;

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
    }

    /// <summary>
    /// Verifies that the token generation process correctly creates a valid token with the expected claims.
    /// </summary>
    [Fact]
    public void WhenGenerateToken_GivenUserAndConfiguration_ReturnWriteTokenValid() 
    {
        //Arrange
        var tokenHandler = new JwtSecurityTokenHandler();

        string userName = "Test_User";
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        //Act
        string token = _tokenService.GenerateToken(authClaims, _configuration);

        var jwtToken = tokenHandler.ReadJwtToken(token);

        //Assert
        Assert.NotNull(token);
        Assert.Contains(jwtToken.Claims, claim => claim.Type == "unique_name" && claim.Value == userName);
        Assert.Contains(jwtToken.Claims, claim => claim.Type == JwtRegisteredClaimNames.Jti);
        Assert.True(jwtToken.ValidFrom > DateTime.Now);
    }
}
