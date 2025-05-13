using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CompetitionConfiguration : IEntityTypeConfiguration<Competition>
{
    public void Configure(EntityTypeBuilder<Competition> entity)
    {
        entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
        entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
        entity.Property(e => e.ResultsToken).IsRequired().HasMaxLength(64);
        entity.HasIndex(e => e.ResultsToken).IsUnique();
    }
}
