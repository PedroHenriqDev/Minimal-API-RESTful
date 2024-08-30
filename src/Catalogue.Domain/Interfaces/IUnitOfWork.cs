namespace Catalogue.Domain.Interfaces;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    Task<TTransaction> BeginTransactionAsync<TTransaction>();
    Task CommitAsync();
}
