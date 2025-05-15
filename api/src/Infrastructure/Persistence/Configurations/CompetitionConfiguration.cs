using Fishio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations;

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

        builder.Property(c => c.LocationText).HasColumnType("text");
        builder.Property(c => c.RulesText).HasColumnType("text");
        builder.Property(c => c.ImageUrl).HasColumnType("text");

        builder.Property(c => c.Type)
            .HasConversion(new EnumToStringConverter<CompetitionType>());

        builder.Property(c => c.Status)
            .HasConversion(new EnumToStringConverter<CompetitionStatus>());

        builder.HasOne(c => c.Organizer)
            .WithMany() // User nie musi mieć kolekcji CompetitionsOrganized
            .HasForeignKey(c => c.OrganizerId)
            .OnDelete(DeleteBehavior.Cascade); // Jeśli usuniemy organizatora, jego zawody też? Lub Restrict.

        // Relacja do CompetitionCategory jest konfigurowana w CompetitionCategoryConfiguration
        // builder.HasMany(c => c.CompetitionCategories)
        //    .WithOne(cc => cc.Competition)
        //    .HasForeignKey(cc => cc.CompetitionId);

        // Relacja do Participants
        builder.HasMany(c => c.Participants)
            .WithOne(p => p.Competition)
            .HasForeignKey(p => p.CompetitionId);

        // Relacja do FishCatches
        builder.HasMany(c => c.FishCatches)
            .WithOne(fc => fc.Competition)
            .HasForeignKey(fc => fc.CompetitionId);
    }
}
