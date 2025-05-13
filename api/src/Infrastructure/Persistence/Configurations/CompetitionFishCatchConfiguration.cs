using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CompetitionFishCatchConfiguration : IEntityTypeConfiguration<CompetitionFishCatch>
{
    public void Configure(EntityTypeBuilder<CompetitionFishCatch> entity)
    {
        entity.Property(e => e.SpeciesName).IsRequired().HasMaxLength(255);
        entity.Property(e => e.PhotoUrl).IsRequired();
        entity.Property(e => e.LengthCm).HasColumnType("decimal(6, 2)");
        entity.Property(e => e.WeightKg).HasColumnType("decimal(7, 3)");

        entity.HasOne(d => d.Competition)
              .WithMany(p => p.FishCatches)
              .HasForeignKey(d => d.CompetitionId)
              .OnDelete(DeleteBehavior.Cascade); // Jeśli usuniemy zawody, usuwamy zgłoszenia

        entity.HasOne(d => d.Participant)
              .WithMany(p => p.FishCatches)
              .HasForeignKey(d => d.ParticipantId)
              .OnDelete(DeleteBehavior.Cascade); // Jeśli usuniemy uczestnika z zawodów, usuwamy jego zgłoszenia

        entity.HasOne(d => d.Judge)
              .WithMany(p => p.JudgedFishCatches)
              .HasForeignKey(d => d.JudgeId)
              .OnDelete(DeleteBehavior.Restrict); // Nie usuwaj sędziego, jeśli ma zgłoszenia
    }
}
