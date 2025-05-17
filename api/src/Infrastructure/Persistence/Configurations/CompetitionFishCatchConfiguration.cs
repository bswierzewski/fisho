using Fishio.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class CompetitionFishCatchConfiguration : IEntityTypeConfiguration<CompetitionFishCatch>
{
    public void Configure(EntityTypeBuilder<CompetitionFishCatch> builder)
    {
        builder.Property(e => e.ImageUrl).IsRequired();
        builder.Property(e => e.FishSpeciesId).IsRequired();

        builder.Property(cfc => cfc.Length)
               .HasConversion(
                   l => l == null ? (decimal?)null : l.Value,
                   value => value == null ? null : new FishLength(value.Value))
               .HasColumnName("LengthCm");

        builder.Property(cfc => cfc.Weight)
               .HasConversion(
                   w => w == null ? (decimal?)null : w.Value,
                   value => value == null ? null : new FishWeight(value.Value))
               .HasColumnName("WeightKg");

        builder.HasOne(d => d.Competition)
                  .WithMany(p => p.FishCatches)
                  .HasForeignKey(d => d.CompetitionId)
                  .OnDelete(DeleteBehavior.Cascade); // Jeśli usuniemy zawody, usuwamy zgłoszenia

        builder.HasOne(d => d.Participant)
              .WithMany(p => p.FishCatches)
              .HasForeignKey(d => d.ParticipantId)
              .OnDelete(DeleteBehavior.Cascade); // Jeśli usuniemy uczestnika z zawodów, usuwamy jego zgłoszenia

        builder.HasOne(d => d.Judge)
              .WithMany(p => p.JudgedFishCatches)
              .HasForeignKey(d => d.JudgeId)
              .OnDelete(DeleteBehavior.Restrict); // Nie usuwaj sędziego, jeśli ma zgłoszenia

        builder.HasOne(d => d.FishSpecies)
                .WithMany()
                .HasForeignKey(d => d.FishSpeciesId)
                .OnDelete(DeleteBehavior.Restrict); // Nie usuwaj gatunku, jeśli istnieją zgłoszenia połowów tego gatunku
    }
}
