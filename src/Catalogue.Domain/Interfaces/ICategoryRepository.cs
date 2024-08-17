using Catalogue.Domain.Entities;

namespace Catalogue.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<Category> GetCategoryAsync(Func<bool, Category> func);
    Task UpdateCategoryAsync();
}
