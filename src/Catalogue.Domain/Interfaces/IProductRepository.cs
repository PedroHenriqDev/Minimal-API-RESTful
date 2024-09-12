using Catalogue.Domain.Entities;

namespace Catalogue.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByIdWithCategoryAsync(Guid id);
    Task<Product?> GetByIdAsync(Guid id);
}
