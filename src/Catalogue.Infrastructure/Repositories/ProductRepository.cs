using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Abstractions;
using Catalogue.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.Infrastructure.Repositories;

public sealed class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {}

    public async Task<Product?> GetByIdWithCategoryAsync(int id)
    {
        return await set.Select(p => new Product 
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            CreatedAt = p.CreatedAt,
            CategoryId = p.CategoryId,
            ImageUrl = p.ImageUrl,
            Price = p.Price,
            
            Category = new Category 
            {
                Id = p.Category.Id,
                CreatedAt = p.Category.CreatedAt,
                Description = p.Category.Description,
                Name = p.Category.Name,
            },
        }).FirstOrDefaultAsync(p => p.Id == id);
    }
}
