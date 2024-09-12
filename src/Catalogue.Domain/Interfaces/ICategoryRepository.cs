using Catalogue.Domain.Entities;

namespace Catalogue.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByIdWithProductsAsync(Guid id);
    Task<Category?> GetByIdAsync(Guid id);
}
