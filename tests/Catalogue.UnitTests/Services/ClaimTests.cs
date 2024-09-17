using Catalogue.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Catalogue.Domain.Enums;
using Catalogue.Application.DTOs.Responses;
using System.Security.Claims;
using Bogus;

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
        var user = new Faker<UserResponse>()
            .RuleFor(u => u.BirthDate, (f, u) => f.Date.Past(70, DateTime.Now.AddYears(-13)))
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Name, f => f.Name.FirstName())
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.Role, f => Role.User).Generate();
     
        //Act
        List<Claim> authClaims = _claimService.CreateAuthClaims(user);

        //Assert
        Assert.NotNull(authClaims);
        Assert.NotEmpty(authClaims);
        Assert.Contains(authClaims, c => c.Value == user.Name && c.Type == ClaimTypes.Name);
    }
}
