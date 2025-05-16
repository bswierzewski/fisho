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

        // --- HasData ---
        builder.HasData(
            // --- GŁÓWNE KATEGORIE PUNKTACYJNE ---
            new CategoryDefinition(
                name: "Najdłuższa Ryba (Indywidualnie)",
                description: "Zwycięzcą zostaje uczestnik, który złowił rybę o największej długości.",
                isGlobal: true,
                type: CategoryType.MainScoring,
                metric: CategoryMetric.LengthCm,
                calculationLogic: CategoryCalculationLogic.MaxValue,
                entityType: CategoryEntityType.FishCatch,
                requiresSpecificFishSpecies: false,
                allowManualWinnerAssignment: false
            )
            { Id = 1 },

            new CategoryDefinition(
                name: "Najcięższa Ryba (Indywidualnie)",
                description: "Zwycięzcą zostaje uczestnik, który złowił rybę o największej wadze.",
                isGlobal: true,
                type: CategoryType.MainScoring,
                metric: CategoryMetric.WeightKg,
                calculationLogic: CategoryCalculationLogic.MaxValue,
                entityType: CategoryEntityType.FishCatch,
                requiresSpecificFishSpecies: false,
                allowManualWinnerAssignment: false
            )
            { Id = 2 },

            new CategoryDefinition(
                name: "Suma Długości Złowionych Ryb",
                description: "Zwycięzcą zostaje uczestnik z największą sumą długości wszystkich swoich złowionych ryb.",
                isGlobal: true,
                type: CategoryType.MainScoring,
                metric: CategoryMetric.LengthCm,
                calculationLogic: CategoryCalculationLogic.SumValue,
                entityType: CategoryEntityType.ParticipantAggregateCatches,
                requiresSpecificFishSpecies: false,
                allowManualWinnerAssignment: false
            )
            { Id = 3 },

            new CategoryDefinition(
                name: "Suma Wag Złowionych Ryb",
                description: "Zwycięzcą zostaje uczestnik z największą sumą wag wszystkich swoich złowionych ryb.",
                isGlobal: true,
                type: CategoryType.MainScoring,
                metric: CategoryMetric.WeightKg,
                calculationLogic: CategoryCalculationLogic.SumValue,
                entityType: CategoryEntityType.ParticipantAggregateCatches,
                requiresSpecificFishSpecies: false,
                allowManualWinnerAssignment: false
            )
            { Id = 4 },

            new CategoryDefinition(
                name: "Liczba Złowionych Ryb",
                description: "Zwycięzcą zostaje uczestnik, który złowił najwięcej ryb (sztuk).",
                isGlobal: true,
                type: CategoryType.MainScoring,
                metric: CategoryMetric.FishCount,
                calculationLogic: CategoryCalculationLogic.SumValue, // Suma "1" za każdą rybę
                entityType: CategoryEntityType.ParticipantAggregateCatches,
                requiresSpecificFishSpecies: false,
                allowManualWinnerAssignment: false
            )
            { Id = 5 },

            // --- KATEGORIE SPECJALNE / OSIĄGNIĘCIA ---
            new CategoryDefinition(
                name: "Największa Ryba Zawodów (Gatunek Dowolny)",
                description: "Nagroda za największą (najdłuższą lub najcięższą - do ustalenia przez organizatora) rybę zawodów, niezależnie od gatunku.",
                isGlobal: true,
                type: CategoryType.SpecialAchievement,
                metric: CategoryMetric.NotApplicable, // Organizator wybierze czy długość czy waga
                calculationLogic: CategoryCalculationLogic.ManualAssignment, // Na MVP, potem można zautomatyzować
                entityType: CategoryEntityType.FishCatch,
                requiresSpecificFishSpecies: false,
                allowManualWinnerAssignment: true
            )
            { Id = 10 },

            new CategoryDefinition(
                name: "Największa Ryba Określonego Gatunku",
                description: "Nagroda za największą rybę wybranego gatunku (np. Największy Szczupak). Gatunek wybierany przy dodawaniu kategorii do zawodów.",
                isGlobal: true,
                type: CategoryType.SpecialAchievement,
                metric: CategoryMetric.NotApplicable, // Długość/waga zależy od wyboru przy konfiguracji zawodów
                calculationLogic: CategoryCalculationLogic.ManualAssignment,  // Na MVP
                entityType: CategoryEntityType.FishCatch,
                requiresSpecificFishSpecies: true, // Kluczowe!
                allowManualWinnerAssignment: true
            )
            { Id = 11 },

            new CategoryDefinition(
                name: "Pierwsza Złowiona Ryba Zawodów",
                description: "Nagroda dla uczestnika, który jako pierwszy zarejestruje połów.",
                isGlobal: true,
                type: CategoryType.SpecialAchievement,
                metric: CategoryMetric.TimeOfCatch,
                calculationLogic: CategoryCalculationLogic.FirstOccurrence,
                entityType: CategoryEntityType.FishCatch,
                requiresSpecificFishSpecies: false, 
                allowManualWinnerAssignment: false  // System może to wyłonić
            )
            { Id = 12 },

            new CategoryDefinition(
                name: "Najmłodszy Uczestnik z Rybą",
                description: "Nagroda dla najmłodszego uczestnika, który złowił jakąkolwiek rybę.",
                isGlobal: true,
                type: CategoryType.FunChallenge,
                metric: CategoryMetric.NotApplicable,
                calculationLogic: CategoryCalculationLogic.ManualAssignment,  // Wymaga danych o wieku, na MVP manualnie
                entityType: CategoryEntityType.ParticipantProfile,
                requiresSpecificFishSpecies: false,
                allowManualWinnerAssignment: true
            )
            { Id = 13 },

            new CategoryDefinition(
                name: "Największa Różnorodność Gatunków",
                description: "Nagroda dla uczestnika, który złowił najwięcej różnych gatunków ryb.",
                isGlobal: true,
                type: CategoryType.SpecialAchievement,
                metric: CategoryMetric.SpeciesVariety,
                calculationLogic: CategoryCalculationLogic.MaxValue,
                entityType: CategoryEntityType.ParticipantAggregateCatches,
                requiresSpecificFishSpecies: false,
                allowManualWinnerAssignment: false  // System może to policzyć
            )
            { Id = 14 }
        );
    }
}
