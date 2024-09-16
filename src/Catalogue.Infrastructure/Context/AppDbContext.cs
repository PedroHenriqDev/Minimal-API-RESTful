using Catalogue.Domain.Entities;
using Catalogue.Infrastructure.EntitiesConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    protected AppDbContext()
    {}

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new UserConfiguration());
    }
} 
