using Fishio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fishio.Infrastructure.Persistence.Configurations;

public class CategoryDefinitionConfiguration : IEntityTypeConfiguration<CategoryDefinition>
{
    public void Configure(EntityTypeBuilder<CategoryDefinition> builder)
    {
        builder.HasKey(cd => cd.Id);

        builder.Property(cd => cd.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(cd => cd.Description).HasColumnType("text");

        builder.Property(cd => cd.Type)
            .HasConversion(new EnumToStringConverter<CategoryType>());

        builder.Property(cd => cd.Metric)
            .HasConversion(new EnumToStringConverter<CategoryMetric>());

        builder.Property(cd => cd.CalculationLogic)
            .HasConversion(new EnumToStringConverter<CategoryCalculationLogic>());

        builder.Property(cd => cd.EntityType)
            .HasConversion(new EnumToStringConverter<CategoryEntityType>());

        // Relacja zwrotna (opcjonalna, EF Core często wykrywa ją automatycznie)
        builder.HasMany(cd => cd.CompetitionCategories)
            .WithOne(cc => cc.CategoryDefinition)
            .HasForeignKey(cc => cc.CategoryDefinitionId);

        // --- HasData ---
        builder.HasData(
            // --- GŁÓWNE KATEGORIE PUNKTACYJNE ---
            new CategoryDefinition
            {
                Id = 1,
                Name = "Najdłuższa Ryba (Indywidualnie)",
                Description = "Zwycięzcą zostaje uczestnik, który złowił rybę o największej długości.",
                IsGlobal = true,
                Type = CategoryType.MainScoring,
                Metric = CategoryMetric.LengthCm,
                CalculationLogic = CategoryCalculationLogic.MaxValue,
                EntityType = CategoryEntityType.FishCatch, // Dotyczy pojedynczego połowu, ale wynik dla uczestnika
                RequiresSpecificFishSpecies = false,
                AllowManualWinnerAssignment = false, // System powinien to obliczyć
                Created = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LastModified = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new CategoryDefinition
            {
                Id = 2,
                Name = "Najcięższa Ryba (Indywidualnie)",
                Description = "Zwycięzcą zostaje uczestnik, który złowił rybę o największej wadze.",
                IsGlobal = true,
                Type = CategoryType.MainScoring,
                Metric = CategoryMetric.WeightKg,
                CalculationLogic = CategoryCalculationLogic.MaxValue,
                EntityType = CategoryEntityType.FishCatch,
                RequiresSpecificFishSpecies = false,
                AllowManualWinnerAssignment = false,
                Created = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LastModified = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new CategoryDefinition
            {
                Id = 3,
                Name = "Suma Długości Złowionych Ryb",
                Description = "Zwycięzcą zostaje uczestnik z największą sumą długości wszystkich swoich złowionych ryb.",
                IsGlobal = true,
                Type = CategoryType.MainScoring,
                Metric = CategoryMetric.LengthCm,
                CalculationLogic = CategoryCalculationLogic.SumValue,
                EntityType = CategoryEntityType.ParticipantAggregateCatches,
                RequiresSpecificFishSpecies = false,
                AllowManualWinnerAssignment = false,
                Created = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LastModified = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new CategoryDefinition
            {
                Id = 4,
                Name = "Suma Wag Złowionych Ryb",
                Description = "Zwycięzcą zostaje uczestnik z największą sumą wag wszystkich swoich złowionych ryb.",
                IsGlobal = true,
                Type = CategoryType.MainScoring,
                Metric = CategoryMetric.WeightKg,
                CalculationLogic = CategoryCalculationLogic.SumValue,
                EntityType = CategoryEntityType.ParticipantAggregateCatches,
                RequiresSpecificFishSpecies = false,
                AllowManualWinnerAssignment = false,
                Created = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LastModified = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new CategoryDefinition
            {
                Id = 5,
                Name = "Liczba Złowionych Ryb",
                Description = "Zwycięzcą zostaje uczestnik, który złowił najwięcej ryb (sztuk).",
                IsGlobal = true,
                Type = CategoryType.MainScoring,
                Metric = CategoryMetric.FishCount,
                CalculationLogic = CategoryCalculationLogic.SumValue, // Suma "1" za każdą rybę
                EntityType = CategoryEntityType.ParticipantAggregateCatches,
                RequiresSpecificFishSpecies = false,
                AllowManualWinnerAssignment = false,
                Created = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LastModified = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            // --- KATEGORIE SPECJALNE / OSIĄGNIĘCIA ---
            new CategoryDefinition
            {
                Id = 10,
                Name = "Największa Ryba Zawodów (Gatunek Dowolny)",
                Description = "Nagroda za największą (najdłuższą lub najcięższą - do ustalenia przez organizatora) rybę zawodów, niezależnie od gatunku.",
                IsGlobal = true,
                Type = CategoryType.SpecialAchievement,
                Metric = CategoryMetric.NotApplicable, // Organizator wybierze czy długość czy waga
                CalculationLogic = CategoryCalculationLogic.ManualAssignment, // Na MVP, potem można zautomatyzować
                EntityType = CategoryEntityType.FishCatch,
                RequiresSpecificFishSpecies = false,
                AllowManualWinnerAssignment = true,
                Created = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LastModified = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new CategoryDefinition
            {
                Id = 11,
                Name = "Największa Ryba Określonego Gatunku",
                Description = "Nagroda za największą rybę wybranego gatunku (np. Największy Szczupak). Gatunek wybierany przy dodawaniu kategorii do zawodów.",
                IsGlobal = true,
                Type = CategoryType.SpecialAchievement,
                Metric = CategoryMetric.NotApplicable, // Długość/waga zależy od wyboru przy konfiguracji zawodów
                CalculationLogic = CategoryCalculationLogic.ManualAssignment, // Na MVP
                EntityType = CategoryEntityType.FishCatch,
                RequiresSpecificFishSpecies = true, // Kluczowe!
                AllowManualWinnerAssignment = true,
                Created = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LastModified = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new CategoryDefinition
            {
                Id = 12,
                Name = "Pierwsza Złowiona Ryba Zawodów",
                Description = "Nagroda dla uczestnika, który jako pierwszy zarejestruje połów.",
                IsGlobal = true,
                Type = CategoryType.SpecialAchievement,
                Metric = CategoryMetric.TimeOfCatch,
                CalculationLogic = CategoryCalculationLogic.FirstOccurrence,
                EntityType = CategoryEntityType.FishCatch,
                RequiresSpecificFishSpecies = false,
                AllowManualWinnerAssignment = false, // System może to wyłonić
                Created = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LastModified = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new CategoryDefinition
            {
                Id = 13,
                Name = "Najmłodszy Uczestnik z Rybą",
                Description = "Nagroda dla najmłodszego uczestnika, który złowił jakąkolwiek rybę.",
                IsGlobal = true,
                Type = CategoryType.FunChallenge,
                Metric = CategoryMetric.NotApplicable,
                CalculationLogic = CategoryCalculationLogic.ManualAssignment, // Wymaga danych o wieku, na MVP manualnie
                EntityType = CategoryEntityType.ParticipantProfile,
                RequiresSpecificFishSpecies = false,
                AllowManualWinnerAssignment = true,
                Created = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LastModified = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new CategoryDefinition
            {
                Id = 14,
                Name = "Największa Różnorodność Gatunków",
                Description = "Nagroda dla uczestnika, który złowił najwięcej różnych gatunków ryb.",
                IsGlobal = true,
                Type = CategoryType.SpecialAchievement,
                Metric = CategoryMetric.SpeciesVariety,
                CalculationLogic = CategoryCalculationLogic.MaxValue,
                EntityType = CategoryEntityType.ParticipantAggregateCatches,
                RequiresSpecificFishSpecies = false,
                AllowManualWinnerAssignment = false, // System może to policzyć
                Created = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LastModified = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
