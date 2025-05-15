using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ScoringCategoryOptionConfiguration : IEntityTypeConfiguration<ScoringCategoryOption>
{
    public void Configure(EntityTypeBuilder<ScoringCategoryOption> builder)
    {
        builder.HasKey(sco => sco.Id); // Upewnij się, że klucz główny jest zdefiniowany

        builder.Property(sco => sco.Name)
            .IsRequired();

        builder.Property(sco => sco.Description);

        // Dodawanie danych początkowych (seed data)
        builder.HasData(
            new ScoringCategoryOption("Suma wag", "Ranking na podstawie sumy wag wszystkich złowionych ryb (w kg).") { Id = 1, },
            new ScoringCategoryOption("Suma długości", "Ranking na podstawie sumy długości wszystkich złowionych ryb (w cm).") { Id = 2 },
            new ScoringCategoryOption("Najdłuższa ryba", "Ranking na podstawie długości pojedynczej, najdłuższej ryby.") { Id = 3 },
            new ScoringCategoryOption("Najcięższa ryba", "Ranking na podstawie wagi pojedynczej, najcięższej ryby.") { Id = 4 },
            new ScoringCategoryOption("Ilość sztuk", "Ranking na podstawie łącznej liczby złowionych ryb (wymiarowych/wszystkich - do ustalenia w regulaminie).") { Id = 5 }            
        );
    }
}