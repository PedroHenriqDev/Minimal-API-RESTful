using Catalogue.Domain.Entities;

namespace Catalogue.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity 
{
    Task<Product> GetAllAsync();
    Task<Product> AddAsync(TEntity entity);
    Task<Product> DeleteAsync(int id);
    Task<Product> UpdateAsync(Product entity);
}
