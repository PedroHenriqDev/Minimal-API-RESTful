using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Repositories;
using Catalogue.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.IntegrationTests.Repositories;

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
    /// Verifies that the 'GetByIdWithCategories' method retrieves the correct 'Product' 
    /// along with its associated 'Category'.
    /// </summary>
    [Fact]
    public async Task GetByIdProductWithCategory_WhenProductIdExists_ReturnProductWithExpectedCategory()
    {
        //Arrange
        Product productExists = _productRepository.GetAll().First(); 

        //Act
        Product? product = await _productRepository.GetByIdWithCategoryAsync(productExists.Id);

        //Assert
        Assert.NotNull(product);
        Assert.NotNull(product.Category);
        Assert.Equal(product.Category.Name, productExists.Category.Name);
    }
}