using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class LogbookEntryConfiguration : IEntityTypeConfiguration<LogbookEntry>
{
    public void Configure(EntityTypeBuilder<LogbookEntry> builder)
    {
        builder.Property(e => e.SpeciesName).IsRequired().HasMaxLength(255);
        builder.Property(e => e.PhotoUrl).IsRequired();
        builder.Property(e => e.LengthCm).HasColumnType("decimal(6, 2)");
        builder.Property(e => e.WeightKg).HasColumnType("decimal(7, 3)");

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
