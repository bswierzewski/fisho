using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class FishSpeciesConfiguration : IEntityTypeConfiguration<FishSpecies>
{
    public void Configure(EntityTypeBuilder<FishSpecies> builder)
    {
        builder.HasKey(fs => fs.Id);

        builder.Property(fs => fs.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(fs => fs.Name).IsUnique();

        builder.HasData(
            new FishSpecies("Szczupak") { Id = 1 },
            new FishSpecies("Okoń") { Id = 2 },
            new FishSpecies("Płoć") { Id = 3 },
            new FishSpecies("Leszcz") { Id = 4 },
            new FishSpecies("Sandacz") { Id = 5 },
            new FishSpecies("Węgorz") { Id = 6 },
            new FishSpecies("Karaś") { Id = 7 },
            new FishSpecies("Lin") { Id = 8 },
            new FishSpecies("Karp") { Id = 9 },
            new FishSpecies("Pstrąg") { Id = 10 },
            new FishSpecies("Jaź") { Id = 11 },
            new FishSpecies("Ukleja") { Id = 12 },
            new FishSpecies("Sum") { Id = 13 },
            new FishSpecies("Wzdręga") { Id = 14 },
            new FishSpecies("Boleń") { Id = 15 }
        );
    }
}
