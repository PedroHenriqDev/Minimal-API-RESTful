using Catalogue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogue.Infrastructure.EntitiesConfiguration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(255);

        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId);
    }
}
