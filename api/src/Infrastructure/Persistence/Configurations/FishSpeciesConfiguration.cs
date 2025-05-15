using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FishSpeciesConfiguration : IEntityTypeConfiguration<FishSpecies>
{
    public void Configure(EntityTypeBuilder<FishSpecies> builder)
    {
        entity.HasKey(fs => fs.Id); // Upewnij się, że klucz główny jest zdefiniowany

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255);

        entity.HasIndex(e => e.Name)
            .IsUnique();

        entity.HasData(
            new FishSpecies("Szczupak") { Id = 1 },
            new FishSpecies("Okoń") { Id = 2 },
            new FishSpecies("Sandacz") { Id = 3 },
            new FishSpecies("Karp") { Id = 4 },
            new FishSpecies("Leszcz") { Id = 5 },
            new FishSpecies("Płoć") { Id = 6 },
            new FishSpecies("Lin") { Id = 7 },
            new FishSpecies("Sum") { Id = 8 },
            new FishSpecies("Węgorz") { Id = 9 },
            new FishSpecies("Pstrąg potokowy") { Id = 10 }
        );
    }
}
