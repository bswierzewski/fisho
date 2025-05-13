using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CompetitionParticipantConfiguration : IEntityTypeConfiguration<CompetitionParticipant>
{
    public void Configure(EntityTypeBuilder<CompetitionParticipant> entity)
    {
        entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
        entity.HasIndex(e => new { e.CompetitionId, e.UserId }).IsUnique().HasFilter("\"UserId\" IS NOT NULL");
        entity.HasIndex(e => new { e.CompetitionId, e.GuestIdentifier }).IsUnique().HasFilter("\"GuestIdentifier\" IS NOT NULL");

        entity.HasOne(d => d.Competition)
              .WithMany(p => p.Participants)
              .HasForeignKey(d => d.CompetitionId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(d => d.User)
              .WithMany(p => p.CompetitionParticipations)
              .HasForeignKey(d => d.UserId)
              .IsRequired(false) // Bo może być gość
              .OnDelete(DeleteBehavior.SetNull);
    }
}
