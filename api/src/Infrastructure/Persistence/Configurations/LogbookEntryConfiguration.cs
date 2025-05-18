using Fishio.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class LogbookEntryConfiguration : IEntityTypeConfiguration<LogbookEntry>
{
    public void Configure(EntityTypeBuilder<LogbookEntry> builder)
    {
        builder.HasKey(le => le.Id);

        builder.Property(le => le.ImageUrl)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(le => le.CatchTime).IsRequired();

        // Konfiguracja Value Object FishLength
        builder.Property(le => le.Length)
            .HasConversion(
                l => l == null ? (decimal?)null : l.Value,
                value => value == null ? null : new FishLength(value.Value))
            .HasColumnName("LengthCm")
            .HasPrecision(7, 2); // np. 99999.99 cm

        // Konfiguracja Value Object FishWeight
        builder.Property(le => le.Weight)
            .HasConversion(
                w => w == null ? (decimal?)null : w.Value,
                value => value == null ? null : new FishWeight(value.Value))
            .HasColumnName("WeightKg")
            .HasPrecision(7, 3); // np. 9999.999 kg

        builder.Property(le => le.Notes)
            .HasMaxLength(2000);

        // Relacja z User (właściciel wpisu)
        builder.HasOne(le => le.User)
            .WithMany(u => u.LogbookEntries)
            .HasForeignKey(le => le.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // Jeśli User zostanie usunięty, jego wpisy też

        // Relacja z FishSpecies (opcjonalna)
        builder.HasOne(le => le.FishSpecies)
            .WithMany(fs => fs.LogbookEntries)
            .HasForeignKey(le => le.FishSpeciesId)
            .OnDelete(DeleteBehavior.SetNull); // Jeśli gatunek zostanie usunięty, FishSpeciesId staje się null

        // Relacja z Fishery (opcjonalna)
        builder.HasOne(le => le.Fishery)
            .WithMany(f => f.LogbookEntries)
            .HasForeignKey(le => le.FisheryId)
            .OnDelete(DeleteBehavior.SetNull); // Jeśli łowisko zostanie usunięte, FisheryId staje się null

        // Konfiguracja BaseAuditableEntity
        builder.Property(le => le.Created).IsRequired();
        builder.Property(le => le.LastModified).IsRequired();
    }
}
