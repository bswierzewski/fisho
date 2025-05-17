using System.ComponentModel;

namespace Fishio.Domain.Enums;

public enum CategoryMetric
{
    [Description("Dla kategorii czysto opisowych/manualnych")]
    NotApplicable = 0,

    [Description("Długość w cm")]
    LengthCm = 1,

    [Description("Waga w kg")]
    WeightKg = 2,

    [Description("Liczba złowionych ryb")]
    FishCount = 3,

    [Description("Różnorodność gatunków")]
    SpeciesVariety = 4,

    [Description("Czas połowu")]
    TimeOfCatch = 5
}
