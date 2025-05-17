using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class CompetitionFishCatchConfiguration : IEntityTypeConfiguration<CompetitionFishCatch>
{
      public void Configure(EntityTypeBuilder<CompetitionFishCatch> builder)
      {
            builder.Property(e => e.ImageUrl).IsRequired();
            builder.Property(e => e.FishSpeciesId).IsRequired();
            builder.Property(e => e.Length).HasColumnType("decimal(6, 2)");
            builder.Property(e => e.Weight).HasColumnType("decimal(7, 3)");

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
