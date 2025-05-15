using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FishSpeciesConfiguration : IEntityTypeConfiguration<FishSpecies>
{
    public void Configure(EntityTypeBuilder<FishSpecies> builder)
    {
        builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
        builder.HasIndex(e => e.Name).IsUnique();
    }
}
