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

    [Fact]
    public void GetAllUsers_ReturnUsers() 
    {
        //Act
        List<User> users = _userRepository.GetAll().ToList();

        Assert.NotEmpty(users);
    }
}
