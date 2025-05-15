namespace Fishio.Domain.Enums;

// Jak wyłaniany jest zwycięzca
public enum CategoryCalculationLogic
{
    ManualAssignment = 0, // Ręczne przypisanie zwycięzcy
    MaxValue = 1,         // Największa wartość (np. najdłuższa ryba)
    SumValue = 2,         // Suma wartości (np. suma wag wszystkich ryb)
    MinValue = 3,         // Najmniejsza wartość (np. najkrótszy czas do złowienia)
    FirstOccurrence = 4,  // Pierwsze wystąpienie (np. pierwsza złowiona ryba)
    LastOccurrence = 5    // Ostatnie wystąpienie
}
