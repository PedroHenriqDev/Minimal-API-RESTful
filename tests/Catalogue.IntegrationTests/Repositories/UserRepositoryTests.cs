using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Repositories;
using Catalogue.IntegrationTests.Fixtures;

namespace Catalogue.IntegrationTests.Repositories;

public class UserRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly IUserRepository _userRepository;

    public UserRepositoryTests(DatabaseFixture databaseFixture)
    {
        _userRepository = new UserRepository(databaseFixture.DbContext);
    }

    /// <summary>
    /// Tests that the 'GetAll' method of the 'UserRepository' returns a non-empty list of users.
    /// </summary>
    /// </summary>
    [Fact]
    public void GetAllUsers_ReturnUsers() 
    {
        //Act
        IQueryable<User> users = _userRepository.GetAll();

        Assert.NotNull(users);
        Assert.NotEmpty(users);
    }
}
