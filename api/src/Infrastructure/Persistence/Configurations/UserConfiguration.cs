using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(e => e.ClerkUserId).IsUnique();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
        builder.Property(e => e.Email).HasMaxLength(255);

        // Unikalny email, jeśli nie jest null
        builder.HasIndex(e => e.Email).IsUnique().HasFilter("\"Email\" IS NOT NULL");
    }
}
