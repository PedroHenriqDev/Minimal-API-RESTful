using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Repositories;
using Catalogue.IntegrationTests.Fixtures;

namespace Catalogue.IntegrationTests.Repositories;

[Collection(nameof(DatabaseFixture))]
public class ProductRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _dbFixture;
    private readonly IProductRepository _productRepository;

    public ProductRepositoryTests(DatabaseFixture dbFixture)
    {
        _dbFixture = dbFixture;
        _productRepository = new ProductRepository(_dbFixture.DbContext);
    }

    /// <summary>
    /// Verifies that the 'GetByIdWithCategoriesAsync' method retrieves the correct 'Product' 
    /// along with its associated 'Category'.
    /// </summary>
    [Fact]
    public async Task GetByIdProductWithCategory_WhenProductIdExists_ReturnProductWithExpectedCategory()
    {
        //Arrange
        Product? productExpected = await _productRepository.GetAsNoTrackingAsync(p => p != null);

        //Act
        Product? product = await _productRepository.GetByIdWithCategoryAsync(productExpected.Id);

        //Assert
        Assert.NotNull(product);
        Assert.Equal(productExpected.Id, product.Id);
        Assert.Equal(product?.Category?.Id, productExpected.CategoryId);
    }
}