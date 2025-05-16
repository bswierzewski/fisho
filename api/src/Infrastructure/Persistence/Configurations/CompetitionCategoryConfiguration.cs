using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class CompetitionCategoryConfiguration : IEntityTypeConfiguration<CompetitionCategory>
{
    public void Configure(EntityTypeBuilder<CompetitionCategory> builder)
    {
        builder.HasKey(cc => cc.Id);

        builder.HasOne(cc => cc.Competition)
            .WithMany(c => c.CompetitionCategories)
            .HasForeignKey(cc => cc.CompetitionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacja do CategoryDefinition
        builder.HasOne(cc => cc.CategoryDefinition) // Strona "jeden"
            .WithMany() // Strona "wiele" - nie ma właściwości nawigacyjnej w CategoryDefinition, więc używamy .WithMany() bez argumentu
            .HasForeignKey(cc => cc.CategoryDefinitionId) // Definiujemy klucz obcy
            .IsRequired() // CategoryDefinitionId jest NOT NULL
            .OnDelete(DeleteBehavior.Restrict); // WAŻNE: Zachowujemy to, aby nie można było usunąć używanej definicji

        builder.HasOne(cc => cc.SpecificFishSpecies)
            .WithMany() // FishSpecies nie potrzebuje kolekcji CompetitionCategories
            .HasForeignKey(cc => cc.SpecificFishSpeciesId)
            .OnDelete(DeleteBehavior.SetNull); // Jeśli gatunek zostanie usunięty, kategoria pozostaje, ale bez gatunku

        builder.Property(cc => cc.CustomNameOverride).HasMaxLength(255);
        builder.Property(cc => cc.CustomDescriptionOverride).HasColumnType("text");

        // Unikalny klucz, aby ta sama definicja kategorii (opcjonalnie dla tego samego gatunku)
        // nie była dodana wielokrotnie do tych samych zawodów.
        // Jeśli SpecificFishSpeciesId jest NULL, to też jest brane pod uwagę w unikalności.
        builder.HasIndex(cc => new { cc.CompetitionId, cc.CategoryDefinitionId, cc.SpecificFishSpeciesId })
            .IsUnique()
            .HasFilter("\"SpecificFishSpeciesId\" IS NOT NULL"); // Dla PostgreSQL, gdy SpecificFishSpeciesId jest częścią klucza

        builder.HasIndex(cc => new { cc.CompetitionId, cc.CategoryDefinitionId })
            .IsUnique()
            .HasFilter("\"SpecificFishSpeciesId\" IS NULL"); // Dla PostgreSQL, gdy SpecificFishSpeciesId nie jest częścią klucza
    }
}
