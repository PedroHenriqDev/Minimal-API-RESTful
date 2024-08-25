using Catalogue.Domain.Entities;

namespace Catalogue.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    public IQueryable<Category> GetAllWithProducts();     
}
