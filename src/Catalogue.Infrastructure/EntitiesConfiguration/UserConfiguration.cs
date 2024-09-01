using Catalogue.Domain.Entities;
using Catalogue.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogue.Infrastructure.EntitiesConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder.Property(u => u.Name).HasMaxLength(255).IsRequired();

        builder.Property(u => u.Email).HasMaxLength(256);

        builder.Property(u => u.Role).HasDefaultValue(Role.User).HasConversion<string>();

        builder.Property(u => u.Password).HasMaxLength(256).IsRequired();
    }
}
