using Catalogue.Domain.Entities;
using Catalogue.Infrastructure.EntitiesConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set;}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new ProductConfiguration());
    }
} 
