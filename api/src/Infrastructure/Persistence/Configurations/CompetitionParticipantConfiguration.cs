using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class CompetitionParticipantConfiguration : IEntityTypeConfiguration<CompetitionParticipant>
{
    public void Configure(EntityTypeBuilder<CompetitionParticipant> builder)
    {
        builder.Property(e => e.Role).IsRequired().HasMaxLength(50);
        builder.HasIndex(e => new { e.CompetitionId, e.UserId }).IsUnique().HasFilter("\"UserId\" IS NOT NULL");
        builder.HasIndex(e => new { e.CompetitionId, e.GuestIdentifier }).IsUnique().HasFilter("\"GuestIdentifier\" IS NOT NULL");

        builder.HasOne(d => d.Competition)
            .WithMany(p => p.Participants)
            .HasForeignKey(d => d.CompetitionId)
            .OnDelete(DeleteBehavior.Cascade); // If we delete the competition, we delete the participants
    }
}
