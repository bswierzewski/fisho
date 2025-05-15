using System.ComponentModel;

namespace Fishio.Domain.Enums;

public enum CategoryMetric
{
    [Description("Dla kategorii czysto opisowych/manualnych")]
    NotApplicable = 0, // Dla kategorii czysto opisowych/manualnych

    [Description("Długość w cm")]
    LengthCm = 1,      // Długość w cm

    [Description("Waga w kg")]
    WeightKg = 2,      // Waga w kg

    [Description("Liczba złowionych ryb")]
    FishCount = 3,     // Liczba złowionych ryb

    [Description("Różnorodność gatunków")]
    SpeciesVariety = 4,// Różnorodność gatunków

    [Description("Czas połowu")]
    TimeOfCatch = 5    // Czas połowu (np. pierwsza/ostatnia ryba)
}
