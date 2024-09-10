using Catalogue.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Catalogue.Domain.Enums;
using Catalogue.Application.DTOs.Responses;
using System.Security.Claims;

namespace Catalogue.UnitTests.Services;

public class ClaimTests
{
    private readonly Mock<ILogger<ClaimService>> _mockedLogger;
    private readonly ClaimService _claimService;

    public ClaimTests()
    {
        _mockedLogger = new Mock<ILogger<ClaimService>>();

        _claimService = new ClaimService(_mockedLogger.Object);
    }

    /// <summary>
    /// Verifies that the generated authentication claims are valid based on the provided user information.
    /// </summary>
    [Fact]
    public void CreateAuthClaims_GivenUser_ReturnAuthClaimsValid()
    {
        //Arrange
        string userName = "Test";

        var user = new UserResponse
        {
            BirthDate = new DateTime(2005, 12, 2),
            Email = "test@gmail.com",
            Name = userName,
            Role = Role.User,
            Id = Guid.NewGuid(),
        };

        //Act
        List<Claim> authClaims = _claimService.CreateAuthClaims(user);

        //Assert
        Assert.NotNull(authClaims);
        Assert.NotEmpty(authClaims);
        Assert.Contains(authClaims, c => c.Value == userName && c.Type == ClaimTypes.Name);
    }
}
