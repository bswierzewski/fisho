namespace Fishio.Domain.Enums;

// Co jest mierzone w danej kategorii
public enum CategoryMetric
{
    NotApplicable = 0, // Dla kategorii czysto opisowych/manualnych
    LengthCm = 1,      // Długość w cm
    WeightKg = 2,      // Waga w kg
    FishCount = 3,     // Liczba złowionych ryb
    SpeciesVariety = 4,// Różnorodność gatunków
    TimeOfCatch = 5    // Czas połowu (np. pierwsza/ostatnia ryba)
}
