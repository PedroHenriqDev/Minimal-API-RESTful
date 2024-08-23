using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Catalogue.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private ICategoryRepository? _categoryRepository;
    private IProductRepository? _productRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public ICategoryRepository CategoryRepository
    {
        get
        {
            return _categoryRepository ?? new CategoryRepository(_context);
        }
    }

    public IProductRepository ProductRepository
    {
        get
        {
            return _productRepository ?? new ProductRepository(_context);
        }
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<TTransaction> BeginTransactionAsync<TTransaction>()
    {
        return (TTransaction)await _context.Database.BeginTransactionAsync();
    }
}
