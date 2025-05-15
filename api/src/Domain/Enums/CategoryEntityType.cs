namespace Fishio.Domain.Enums;

public enum CategoryEntityType
{
    FishCatch = 1,                 // Dotyczy pojedynczego połowu
    ParticipantAggregateCatches = 2, // Dotyczy wszystkich połowów danego uczestnika
    ParticipantProfile = 3         // Dotyczy profilu uczestnika (np. wiek)
}
