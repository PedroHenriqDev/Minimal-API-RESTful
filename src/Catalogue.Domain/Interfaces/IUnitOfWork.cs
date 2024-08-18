namespace Catalogue.Domain.Interfaces;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    Task CommitAsync();
}
