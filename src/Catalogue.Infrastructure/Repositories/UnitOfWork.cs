using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Context;

namespace Catalogue.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private  ICategoryRepository? _categoryRepository;
    private IProductRepository? _productRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public ICategoryRepository CategoryRepository => _categoryRepository ?? new CategoryRepository(_context);
    public IProductRepository ProductRepository => _productRepository ?? new ProductRepository(_context);

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
