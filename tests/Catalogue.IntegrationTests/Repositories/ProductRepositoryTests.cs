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
    private readonly Product firstProduct;

    public ProductRepositoryTests(DatabaseFixture dbFixture)
    {
        _dbFixture = dbFixture;
        _productRepository = new ProductRepository(_dbFixture.DbContext);
        firstProduct = _productRepository.GetAll().Include(p => p.Category).First();
    }

    /// <summary>
    /// Verifies that the 'GetByIdWithCategories' method retrieves the correct 'Product' 
    /// along with its associated 'Category'.
    /// </summary>
    [Fact]
    public async Task GetByIdProductWithCategory_WhenProductIdExists_ReturnProductWithExpectedCategory()
    {
        //Arrange
        Guid productId = firstProduct.Id; 

        //Act
        Product? product = await _productRepository.GetByIdWithCategoryAsync(productId);

        //Assert
        Assert.NotNull(product);
        Assert.NotNull(product.Category);
        Assert.Equal(product.Category.Name, firstProduct.Category.Name);
    }
}