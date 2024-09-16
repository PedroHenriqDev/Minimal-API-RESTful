using Catalogue.Domain.Abstractions;
using System.Linq.Expressions;

namespace Catalogue.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity
{
    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
    void Delete(TEntity entity);
    void Update(TEntity entity);
}
