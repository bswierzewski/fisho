using Fishio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class CompetitionConfiguration : IEntityTypeConfiguration<Competition>
{
    public void Configure(EntityTypeBuilder<Competition> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.ResultsToken)
            .HasMaxLength(64)
            .IsRequired();
        builder.HasIndex(c => c.ResultsToken).IsUnique();

        builder.Property(c => c.Rules).HasColumnType("text");
        builder.Property(c => c.ImageUrl).HasColumnType("text");

        builder.Property(c => c.Type)
            .HasConversion(new EnumToStringConverter<CompetitionType>());

        builder.Property(c => c.Status)
            .HasConversion(new EnumToStringConverter<CompetitionStatus>());

        builder.OwnsOne(c => c.Schedule, scheduleBuilder =>
        {
            scheduleBuilder.Property(s => s.Start).HasColumnName("StartTime");
            scheduleBuilder.Property(s => s.End).HasColumnName("EndTime");
        });

        builder.HasOne(c => c.Fishery)
               .WithMany(f => f.Competitions)
               .HasForeignKey(c => c.FisheryId)
               .IsRequired(); // Jawne określenie, że relacja jest wymagana

        builder.HasOne(c => c.Organizer)
            .WithMany()
            .HasForeignKey(c => c.OrganizerId)
            .OnDelete(DeleteBehavior.Cascade); // Jeśli usuniemy organizatora, jego zawody też

    }
}
