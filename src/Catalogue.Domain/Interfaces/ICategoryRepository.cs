using Catalogue.Domain.Entities;

namespace Catalogue.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    public Task<Category?> GetByIdWithProductsAsync(int id);
}
