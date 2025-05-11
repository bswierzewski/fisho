using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FishSpeciesConfiguration : IEntityTypeConfiguration<FishSpecies>
{
    public void Configure(EntityTypeBuilder<FishSpecies> entity)
    {
        entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        entity.HasIndex(e => e.Name).IsUnique();
    }
}
