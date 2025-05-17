//namespace Fishio.Domain.Entities;

//public class CompetitionStand : BaseAuditableEntity
//{
//    public int CompetitionSectorId { get; private set; }
//    public virtual CompetitionSector CompetitionSector { get; private set; } = null!;

//    public string NumberOrLabel { get; private set; } = string.Empty;
//    public string? Description { get; private set; }

//    public int? CompetitionParticipantId { get; private set; }
//    public virtual CompetitionParticipant? CompetitionParticipant { get; private set; }

//    private CompetitionStand() { }

//    public CompetitionStand(
//        int competitionSectorId,
//        string numberOrLabel,
//        string? description = null,
//        int? competitionParticipantId = null
//    )
//    {
//        CompetitionSectorId = competitionSectorId;
//        NumberOrLabel = numberOrLabel;
//        Description = description;
//        CompetitionParticipantId = competitionParticipantId;
//    }
//}
