using Catalogue.Domain.Entities;

namespace Catalogue.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByIdWithProductsAsync(int id);
}
