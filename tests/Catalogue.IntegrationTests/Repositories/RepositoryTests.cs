using AutoBogus;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Repositories;
using Catalogue.IntegrationTests.Fixtures;
using System.Linq.Expressions;

namespace Catalogue.IntegrationTests.Repositories;

public class RepositoryTests
{
    private static readonly DatabaseFixture _dbFixture;
    private static readonly UserRepository _userRepository;
    private static readonly CategoryRepository _categoryRepository;
    private static readonly ProductRepository _productRepository;
    private static readonly User _firstUser;
    private static readonly Category _firstCategory;
    private static readonly Product _firstProduct;

    static RepositoryTests() 
    {
        _dbFixture = new DatabaseFixture();
        _userRepository = new UserRepository(_dbFixture.DbContext);
        _categoryRepository = new CategoryRepository(_dbFixture.DbContext);
        _productRepository = new ProductRepository(_dbFixture.DbContext);
        _firstUser = _dbFixture.DbContext.Users.First();
        _firstCategory = _dbFixture.DbContext.Categories.First();
        _firstProduct = _dbFixture.DbContext.Products.First();
    }

    /// <summary>
    /// Verifies that the 'GetAll' method of the repository returns a non-empty collection of entities.
    /// </summary>
    /// <typeparam name="T">The type of the entities managed by the repository.</typeparam>
    /// <param name="repository">An instance of the repository implementing the IRepository interface.</param>
    [Theory]
    [MemberData(nameof(CreateRepositories))]
    public void GetAllUsers_ReturnNonEmptyCollection<T>(IRepository<T> repository) where T : class
    {
        //Act
        var entities = repository.GetAll();

        //Assert
        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    /// <summary>
    /// Verifies that the 'GetAsync' method correctly retrieves a entity that matches the provided predicate expression.
    /// Ensures that the repository can find the correct product based on the criteria defined in the predicate.
    /// </summary>
    /// <param name="IRepository<T>">The repository used to retrieve the entity.</param>
    /// <param name="predicate">The expression used to define the criteria for finding the entity.</param>
    [Theory]
    [MemberData(nameof(ProvidesRepositoryAndPredicates))]
    public async Task GetEntity_WhenCalledWithValidExpression_ReturnsMatchingEntity<T>(IRepository<T> repository,
                                                                                        Expression<Func<T, bool>> predicate) 
        where T : class
    {
        //Arrange
        T entityExpected = repository.GetAll().First();

        //Act
        T entity = await repository.GetAsync(predicate);

        //Assert
        Assert.NotNull(entity);
        Assert.Equal(entityExpected, entity);
    }

    /// <summary>
    /// Verifies that the 'AddAsync' method successfully inserts a new entity into the database and ensures 
    /// that the entity is correctly persisted in the context after calling 'SaveChangesAsync'.
    /// This test confirms that the repository can handle entity creation and that the changes are reflected in the database.
    /// </summary>
    /// <typeparam name="T">The type of the entity being operated on by the repository.</typeparam>
    /// <param name="repository">The repository used to perform the add operation for the entity.</param>
    [Theory]
    [MemberData(nameof(CreateRepositories))]
    public async Task AddEntity_WhenCalledWithValidEntity_ShouldSuccessfullyPersisted<T>(IRepository<T> repository) where T
        : class
    {
        //Arrange
        T entity = new AutoFaker<T>().Generate();
        IQueryable<T> entities = repository.GetAll();

        //Act
        await repository.AddAsync(entity);
        await _dbFixture.DbContext.SaveChangesAsync();

        //Assert
        Assert.Contains(entity, entities);
    }


    /// <summary>
    /// Verifies that the 'Delete' method successfully removes a entity from the database.
    /// </summary>
    /// <typeparam name="T">The type of entity being operated on by the repository.</typeparam>
    /// <param name="repository">The repository used to perform the delete operation for the entity</param>
    [Theory]
    [MemberData(nameof(CreateRepositories))]
    public async Task DeleteEntity_WhenGivenAEntity_ShouldRemoveItSuccessfully<T>(IRepository<T> repository) 
        where T : class
    {
        //Arrange
        IQueryable<T> entities = repository.GetAll();
        T entity = repository.GetAll().First();

        //Act
        repository.Delete(entity);
        await _dbFixture.DbContext.SaveChangesAsync();

        //Assert
        Assert.DoesNotContain(entity, entities);
    }

    /// <summary>
    /// Provides test data for repository and predicate expressions used to query entities.
    /// Each entry includes a repository instance (<see cref="IRepository{TEntity}"/>) and a corresponding predicate expression 
    /// for finding specific entities based on their properties such as Id or Name.
    /// This data can be used to test repository methods like 'GetAsync' across different entity types (User, Category, Product).
    /// </summary>
    /// <returns>
    /// A collection of object arrays where each array contains a repository instance 
    /// (<see cref="IRepository{TEntity}"/>) and a predicate expression (<see cref="Expression{Func{TEntity, bool}}"/>)
    /// to be used for querying entities.
    /// </returns>
    public static IEnumerable<object[]> ProvidesRepositoryAndPredicates()
    {
        yield return new object[] { _userRepository, (Expression<Func<User, bool>>)(u => u.Id == _firstUser.Id) };
        yield return new object[] { _userRepository, (Expression<Func<User, bool>>)(u => u.Name == _firstUser.Name) };
        yield return new object[] { _categoryRepository, (Expression<Func<Category, bool>>)(c => c.Id == _firstCategory.Id) };
        yield return new object[] { _categoryRepository, (Expression<Func<Category, bool>>)(c => c.Name == _firstCategory.Name) };
        yield return new object[] { _productRepository, (Expression<Func<Product, bool>>)(p => p.Id == _firstProduct.Id) };
        yield return new object[] { _productRepository, (Expression<Func<Product, bool>>)(p => p.Name == _firstProduct.Name) };
    }
   
    /// <summary>
    /// Provides a collection of repository instances for testing.
    /// Each entry contains an instance of a repository for different entity types.
    /// </summary>
    /// <returns>A collection of object arrays where each array contains a repository instance.</returns>
    public static IEnumerable<object[]> CreateRepositories()
    {
        yield return new object[] {_userRepository };
        yield return new object[] { _categoryRepository };
        yield return new object[] { _productRepository };
    }
}
