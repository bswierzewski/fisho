using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasIndex(e => e.ClerkUserId).IsUnique();
        entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Email).HasMaxLength(255);

        // Unikalny email, jeśli nie jest null
        entity.HasIndex(e => e.Email).IsUnique().HasFilter("\"Email\" IS NOT NULL");
    }
}
