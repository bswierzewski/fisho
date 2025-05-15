using System.ComponentModel;

namespace Fishio.Domain.Enums;

public enum CategoryCalculationLogic
{
    [Description("Ręczne przypisanie zwycięzcy")]
    ManualAssignment = 0, // Ręczne przypisanie zwycięzcy

    [Description("Największa wartość")]
    MaxValue = 1,         // Największa wartość (np. najdłuższa ryba)

    [Description("Największa suma wartości")]
    SumValue = 2,         // Suma wartości (np. suma wag wszystkich ryb)

    [Description("Najmniejsza wartość")]
    MinValue = 3,         // Najmniejsza wartość (np. najkrótszy czas do złowienia)

    [Description("Pierwsze wystąpienie")]
    FirstOccurrence = 4,  // Pierwsze wystąpienie (np. pierwsza złowiona ryba)

    [Description("Ostatnie wystąpienie")]
    LastOccurrence = 5    // Ostatnie wystąpienie
}
