using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class CompetitionConfiguration : IEntityTypeConfiguration<Competition>
{
    public void Configure(EntityTypeBuilder<Competition> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(250);

        // Konfiguracja Value Object DateTimeRange dla Schedule
        builder.OwnsOne(c => c.Schedule, scheduleBuilder =>
        {
            scheduleBuilder.Property(s => s.Start)
                .HasColumnName("StartTime")
                .IsRequired();
            scheduleBuilder.Property(s => s.End)
                .HasColumnName("EndTime")
                .IsRequired();
        });

        builder.Property(c => c.Rules)
            .HasColumnType("text"); // Dla długiego tekstu regulaminu

        builder.Property(c => c.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(c => c.ImageUrl)
            .HasMaxLength(2048);

        builder.Property(c => c.ResultsToken)
            .IsRequired()
            .HasMaxLength(64);
        builder.HasIndex(c => c.ResultsToken).IsUnique();

        // Relacja z User (Organizer)
        builder.HasOne(c => c.Organizer)
            .WithMany(u => u.OrganizedCompetitions)
            .HasForeignKey(c => c.OrganizerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // Nie można usunąć organizatora, jeśli ma zawody

        // Relacja z Fishery
        builder.HasOne(c => c.Fishery)
            .WithMany(f => f.Competitions)
            .HasForeignKey(c => c.FisheryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // Nie można usunąć łowiska, jeśli są na nim zawody

        // Relacje z kolekcjami (CompetitionCategory, CompetitionParticipant, CompetitionFishCatch)
        // są konfigurowane przez EF Core jako one-to-many z odpowiednimi FK w encjach "many".
        // Użycie prywatnych pól dla kolekcji:
        builder.Navigation(c => c.Categories)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_categories");

        builder.Navigation(c => c.Participants)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_participants");

        builder.Navigation(c => c.FishCatches)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_fishCatches");


        // Konfiguracja BaseAuditableEntity
        builder.Property(c => c.Created).IsRequired();
        builder.Property(c => c.LastModified).IsRequired();

    }
}
