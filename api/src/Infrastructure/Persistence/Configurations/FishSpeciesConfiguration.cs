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
    }
}
