using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Catalogue.Infrastructure.Abstractions;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    // Represents the set of entities of type 'TEntity' in the database context.
    protected readonly DbSet<TEntity> entities;

    public Repository(AppDbContext context)
    {
        entities = context.Set<TEntity>();
    }

    /// <summary>
    /// Retrives all entities from specified table of type 'TEntity'.
    /// </summary>
    /// <returns>An <see cref="IQueryable{TEntity}"> containg all entities from the table.</returns>
    public IQueryable<TEntity> GetAll()
    {
        return entities;
    }

    /// <summary>
    /// Retrieves the first entity of type 'TEntity' that satisfies the specified expression.
    /// </summary>
    /// <param name="predicate">The expression used to filter the entities of type 'TEntity'.</param>
    /// <returns>The first entity that matches the predicate, or null if no match is found.</returns>
    /// <summary>
    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await entities.FirstOrDefaultAsync(predicate);
    }

    public async Task<TEntity?> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await entities.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public async Task AddAsync(TEntity entity)
    {
        await entities.AddAsync(entity);
    }

    public void Delete(TEntity entity)
    {
        entities.Remove(entity);
    }

    public void Update(TEntity entity)
    {
        entities.Update(entity);
    }
}
