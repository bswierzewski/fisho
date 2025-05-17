using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class CompetitionCategoryConfiguration : IEntityTypeConfiguration<CompetitionCategory>
{
    public void Configure(EntityTypeBuilder<CompetitionCategory> builder)
    {
        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.CustomNameOverride).HasMaxLength(255);
        builder.Property(cc => cc.CustomDescriptionOverride).HasColumnType("text");

        builder.HasOne(cc => cc.CategoryDefinition)
            .WithMany()
            .HasForeignKey(cc => cc.CategoryDefinitionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // If we delete the category definition, we don't want to delete the competition category. We want to keep the competition category for historical reasons. We can still delete the competition category if we want to.
    }
}
