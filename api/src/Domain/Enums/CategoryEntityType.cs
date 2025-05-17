using System.ComponentModel;

namespace Fishio.Domain.Enums;

public enum CategoryEntityType
{
    [Description("Dotyczy pojedynczego połowu")]
    FishCatch = 1,

    [Description("Dotyczy wszystkich połowów danego uczestnika")]
    ParticipantAggregateCatches = 2,

    [Description("Dotyczy profilu uczestnika (np. wiek)")]
    ParticipantProfile = 3
}
