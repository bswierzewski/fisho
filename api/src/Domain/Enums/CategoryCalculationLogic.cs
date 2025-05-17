using System.ComponentModel;

namespace Fishio.Domain.Enums;

public enum CategoryCalculationLogic
{
    [Description("Ręczne przypisanie zwycięzcy")]
    ManualAssignment = 0,

    [Description("Największa wartość")]
    MaxValue = 1,

    [Description("Największa suma wartości")]
    SumValue = 2,

    [Description("Najmniejsza wartość")]
    MinValue = 3,

    [Description("Pierwsze wystąpienie")]
    FirstOccurrence = 4,

    [Description("Ostatnie wystąpienie")]
    LastOccurrence = 5
}
