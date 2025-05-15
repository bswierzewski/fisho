using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SpecialCompetitionCategoryConfiguration : IEntityTypeConfiguration<SpecialCompetitionCategory>
{
    public void Configure(EntityTypeBuilder<SpecialCompetitionCategory> entity)
    {
    }
}
