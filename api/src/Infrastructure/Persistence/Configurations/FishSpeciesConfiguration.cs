using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class FishSpeciesConfiguration : IEntityTypeConfiguration<FishSpecies>
{
    public void Configure(EntityTypeBuilder<FishSpecies> builder)
    {
        builder.HasKey(fs => fs.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.HasData(
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
