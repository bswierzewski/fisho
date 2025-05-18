using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class FisheryConfiguration : IEntityTypeConfiguration<Fishery>
{
    public void Configure(EntityTypeBuilder<Fishery> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.Location)
            .HasMaxLength(500);

        builder.Property(f => f.ImageUrl)
            .HasMaxLength(2048);

        // Relacja z User (Creator) - opcjonalna
        builder.HasOne(f => f.User)
            .WithMany(u => u.CreatedFisheries)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.SetNull); // Jeśli User zostanie usunięty, UserId w Fishery staje się null

        // Relacja wiele-do-wielu z FishSpecies
        builder.HasMany(f => f.FishSpecies)
            .WithMany(fs => fs.Fisheries)
            .UsingEntity<Dictionary<string, object>>(
                "FisheryFishSpecies", // Nazwa tabeli łączącej
                j => j.HasOne<FishSpecies>().WithMany().HasForeignKey("FishSpeciesId"),
                j => j.HasOne<Fishery>().WithMany().HasForeignKey("FisheryId"),
                j =>
                {
                    j.HasKey("FisheryId", "FishSpeciesId");
                    j.ToTable("FisheryFishSpecies");
                });
        // Użycie prywatnego pola dla kolekcji _fishSpecies
        builder.Navigation(f => f.FishSpecies)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_fishSpecies");


        // Konfiguracja BaseAuditableEntity
        builder.Property(f => f.Created).IsRequired();
        builder.Property(f => f.LastModified).IsRequired();
    }
}
