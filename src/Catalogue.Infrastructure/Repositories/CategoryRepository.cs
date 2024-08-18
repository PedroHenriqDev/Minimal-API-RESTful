using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using Catalogue.Infrastructure.Context;

namespace Catalogue.Infrastructure.Repositories;

public sealed class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {}
}
