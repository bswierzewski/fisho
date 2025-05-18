using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class CompetitionCategoryConfiguration : IEntityTypeConfiguration<CompetitionCategory>
{
    public void Configure(EntityTypeBuilder<CompetitionCategory> builder)
    {
        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.CustomNameOverride).HasMaxLength(150);
        builder.Property(cc => cc.CustomDescriptionOverride).HasMaxLength(1000);
        builder.Property(cc => cc.SortOrder).IsRequired();
        builder.Property(cc => cc.IsPrimaryScoring).IsRequired();
        builder.Property(cc => cc.MaxWinnersToDisplay).IsRequired().HasDefaultValue(1);
        builder.Property(cc => cc.IsEnabled).IsRequired().HasDefaultValue(true);

        // Relacja z Competition
        builder.HasOne(cc => cc.Competition)
            .WithMany(c => c.Categories)
            .HasForeignKey(cc => cc.CompetitionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // Jeśli zawody są usuwane, kategorie też

        // Relacja z CategoryDefinition
        builder.HasOne(cc => cc.CategoryDefinition)
            .WithMany(cd => cd.CompetitionCategories)
            .HasForeignKey(cc => cc.CategoryDefinitionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // Nie można usunąć definicji, jeśli jest używana

        // Relacja z FishSpecies (opcjonalna)
        builder.HasOne(cc => cc.FishSpecies)
            .WithMany(fs => fs.CompetitionCategories) // Dodaj ICollection<CompetitionCategory> CompetitionCategories w FishSpecies
            .HasForeignKey(cc => cc.FishSpeciesId)
            .OnDelete(DeleteBehavior.SetNull);

        // Unikalny indeks dla (CompetitionId, CategoryDefinitionId, FishSpeciesId), aby uniknąć duplikatów tej samej kategorii w zawodach
        // FishSpeciesId może być null, więc trzeba to obsłużyć.
        // Można to zrobić na poziomie aplikacji lub bardziej złożonym indeksem filtrowanym.
        // Prostsze: unikalność (CompetitionId, CategoryDefinitionId) jeśli FishSpeciesId jest null,
        // i (CompetitionId, CategoryDefinitionId, FishSpeciesId) jeśli nie jest null.
        // Dla uproszczenia, na razie pominiemy ten złożony indeks.
        // Można rozważyć indeks na (CompetitionId, SortOrder) dla wydajności sortowania.
        builder.HasIndex(cc => new { cc.CompetitionId, cc.SortOrder });


        // Konfiguracja BaseAuditableEntity
        builder.Property(cc => cc.Created).IsRequired();
        builder.Property(cc => cc.LastModified).IsRequired();
    }
}
