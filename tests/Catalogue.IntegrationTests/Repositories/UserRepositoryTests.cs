using AutoBogus;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Repositories;
using Catalogue.IntegrationTests.Fixtures;
using System.Linq.Expressions;

namespace Catalogue.IntegrationTests.Repositories;

public class UserRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly IUserRepository _userRepository;
    private readonly DatabaseFixture _databaseFixture;
    private static User? firstUser;

    public UserRepositoryTests(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
        _userRepository = new UserRepository(_databaseFixture.DbContext);
         InitializeFirstUser();
    }

    /// <summary>
    /// Intantie the object of type 'User' and name FirstUser.
    /// </summary>
    private void InitializeFirstUser()
    {
        firstUser = _userRepository.GetAll().ToList().First();;
    }

    /// <summary>
    /// Tests that the 'GetAll' method of the 'UserRepository' returns a non-empty list of users.
    /// </summary>
    /// </summary>
    [Fact]
    public void GetAllUsers_ReturnNonEmptyCollection() 
    {
        //Act
        IQueryable<User> users = _userRepository.GetAll();

        //Assert
        Assert.NotNull(users);
        Assert.NotEmpty(users);
    }

    /// <summary>
    /// Verifies that the 'GetAsync' method correctly retrives a user that matches the provides predicate expression
    /// </summary>
    /// <param name="predicate">The expression used to find the user.</param>
    [Theory]
    [MemberData(nameof(GetUserPredicates))]
    public async Task GetUser_WhenCalledWithValidExpression_ReturnsMatchingUser(Expression<Func<User, bool>> predicate) 
    {
        //Act
        User? userFound = await _userRepository.GetAsync(predicate);

        //Assert
        Assert.NotNull(userFound);
        Assert.Equal(firstUser, userFound);
    }

    /// <summary>
    /// Verifies that the 'AddAsync' method successfully inserts a new user into database and that the user.
    /// is correcly persisted in the context after calling 'SaveChangesAsync'.
    /// </summary>
    [Fact]
    public async Task AddUser_WhenCalledWithValidUser_ShouldSuccessfullyPersisted()
    {
        //Arrange
        User user = new AutoFaker<User>().RuleFor(u => u.Email, f => f.Internet.Email()).Generate();

        //Act
        await _userRepository.AddAsync(user);
        await _databaseFixture.DbContext.SaveChangesAsync();

        //Assert
        Assert.Contains(user, _userRepository.GetAll().ToList());
    }

    /// <summary>
    /// Provide a collection of expressions used for testing methods of the 'UserRepository'.
    /// Each expression is designed to query users based on different criteria to ensure the repository method
    /// correctly retrives users matching there criteria.
    /// </summary>
    /// <returns>A collection of object arrays where each array contains a predicate expression to test methods.</returns>
    public static IEnumerable<object[]> GetUserPredicates() 
    {
        // Yield an expression to retrieves a user by matching user's Id.
        yield return new object[] { (Expression<Func<User, bool>>)(u => u.Id == firstUser.Id) };

        // Yield an expression to retrieves a user by matching user's Name.
        yield return new object[] { (Expression<Func<User, bool>>)(u => u.Name == firstUser.Name) };
    }
}
