using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Context;

namespace Catalogue.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private ICategoryRepository? categoryRepository;
    private IProductRepository? productRepository;
    private IUserRepository? userRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public ICategoryRepository CategoryRepository =>
        categoryRepository ?? new CategoryRepository(_context);

    public IProductRepository ProductRepository =>
             productRepository ?? new ProductRepository(_context);
    public IUserRepository UserRepository 
        => userRepository ?? new UserRepository(_context);

    public async Task CommitAsync()
        => await _context.SaveChangesAsync();
    

    public async Task<TTransaction> BeginTransactionAsync<TTransaction>()
        => (TTransaction)await _context.Database.BeginTransactionAsync();
}
