using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class LogbookEntryConfiguration : IEntityTypeConfiguration<LogbookEntry>
{
    public void Configure(EntityTypeBuilder<LogbookEntry> entity)
    {
        entity.Property(e => e.SpeciesName).IsRequired().HasMaxLength(255);
        entity.Property(e => e.PhotoUrl).IsRequired();
        entity.Property(e => e.LengthCm).HasColumnType("decimal(6, 2)");
        entity.Property(e => e.WeightKg).HasColumnType("decimal(7, 3)");

        entity.HasOne(d => d.User)
              .WithMany(p => p.LogbookEntries)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.Cascade); // Jeśli usuniemy użytkownika, usuwamy jego wpisy

        entity.HasOne(d => d.Fishery)
              .WithMany(p => p.LogbookEntries)
              .HasForeignKey(d => d.FisheryId)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.SetNull); // Jeśli usuniemy łowisko,
    }
}
