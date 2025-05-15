using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FisheryConfiguration : IEntityTypeConfiguration<Fishery>
{
    public void Configure(EntityTypeBuilder<Fishery> builder)
    {
        builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
    }
}
