using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(e => e.ClerkUserId).IsUnique();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
        builder.Property(e => e.Email).HasMaxLength(255);

        // Unique email if not null
        builder.HasIndex(e => e.Email).IsUnique().HasFilter("\"Email\" IS NOT NULL");
    }
}
