using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Repositories;
using Catalogue.IntegrationTests.Fixtures;

namespace Catalogue.IntegrationTests.Repositories;

[Collection(nameof(DatabaseFixture))]
public class CategoryRepositoryTests
{
    private readonly DatabaseFixture _dbFixture;
    private readonly ICategoryRepository _categoryRepository;

    public CategoryRepositoryTests(DatabaseFixture dbFixture)
    {
        _dbFixture = dbFixture;
        _categoryRepository = new CategoryRepository(dbFixture.DbContext);
    }

    /// <summary>
    /// Verifies that the method 'GetCategoryByIdWithProductsAsync' correctly retrieves a category
    /// along with its associated products when a valid category ID is provided.
    /// </summary>
    [Fact]
    public async Task GetCategoryByIdWithProducts_WhenValidCategoryId_ReturnCategoryWithExpectedProducts()
    {
        //Arrange
        Category categoryExists = _categoryRepository.GetAll().Where(c => c.Products.Count > 0).First();

        //Act
        Category? category = await _categoryRepository.GetByIdWithProductsAsync(categoryExists.Id);

        //Assert
        Assert.NotNull(category);
        Assert.NotEmpty(category.Products);
    }
}