using System.ComponentModel;

namespace Fishio.Domain.Enums;

public enum CategoryEntityType
{
    [Description("Dotyczy pojedynczego połowu")]
    FishCatch = 1,                 // Dotyczy pojedynczego połowu

    [Description("Dotyczy wszystkich połowów danego uczestnika")]
    ParticipantAggregateCatches = 2, // Dotyczy wszystkich połowów danego uczestnika

    [Description("Dotyczy profilu uczestnika (np. wiek)")]
    ParticipantProfile = 3         // Dotyczy profilu uczestnika (np. wiek)
}
