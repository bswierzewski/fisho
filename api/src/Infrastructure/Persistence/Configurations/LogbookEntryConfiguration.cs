using Fishio.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class LogbookEntryConfiguration : IEntityTypeConfiguration<LogbookEntry>
{
    public void Configure(EntityTypeBuilder<LogbookEntry> builder)
    {
        builder.Property(e => e.ImageUrl).IsRequired();

        builder.Property(le => le.Length)
               .HasConversion(
                   l => l == null ? (decimal?)null : l.Value, // Do bazy
                   value => value == null ? null : new FishLength(value.Value)) // Z bazy
               .HasColumnName("LengthCm");

        builder.Property(le => le.Weight)
               .HasConversion(
                   w => w == null ? (decimal?)null : w.Value,
                   value => value == null ? null : new FishWeight(value.Value))
               .HasColumnName("WeightKg");

        builder.HasOne(d => d.User)
            .WithMany(p => p.LogbookEntries)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Jeśli usuniemy użytkownika, usuwamy jego wpisy

        builder.HasOne(d => d.Fishery)
              .WithMany(p => p.LogbookEntries)
              .HasForeignKey(d => d.FisheryId)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.SetNull); // Jeśli usuniemy łowisko,
    }
}
