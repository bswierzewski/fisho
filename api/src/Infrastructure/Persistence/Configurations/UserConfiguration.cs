using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.ClerkUserId)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(u => u.ClerkUserId).IsUnique();

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Email)
            .HasMaxLength(254); // Standardowa max długość dla email
        builder.HasIndex(u => u.Email).IsUnique().HasFilter("\"Email\" IS NOT NULL");


        builder.Property(u => u.ImageUrl)
            .HasMaxLength(2048);

        // Relacje (są one stroną "one" w wielu relacjach one-to-many)
        // EF Core zazwyczaj dobrze sobie z tym radzi przez konwencję,
        // ale można je jawnie zdefiniować, jeśli jest potrzeba.

        // Konfiguracja BaseAuditableEntity (jeśli nie jest globalna)
        builder.Property(u => u.Created).IsRequired();
        builder.Property(u => u.LastModified).IsRequired();
        // CreatedBy i LastModifiedBy są FK do User.Id, EF Core powinien to wykryć
        // lub można skonfigurować:
        // builder.HasOne<User>().WithMany().HasForeignKey(u => u.CreatedBy).OnDelete(DeleteBehavior.Restrict);
        // builder.HasOne<User>().WithMany().HasForeignKey(u => u.LastModifiedBy).OnDelete(DeleteBehavior.Restrict);
    }
}
