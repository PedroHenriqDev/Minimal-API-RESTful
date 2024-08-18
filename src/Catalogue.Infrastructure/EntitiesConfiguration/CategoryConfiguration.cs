using Catalogue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogue.Infrastructure.EntitiesConfiguration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .HasMaxLength(120)
               .IsRequired();
        
        builder.Property(c => c.Description)
               .HasMaxLength(255);

        builder.Property(c => c.CreatedAt)
               .HasConversion(typeof(DateTime));
    }
}
