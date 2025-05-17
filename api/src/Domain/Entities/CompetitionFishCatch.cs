namespace Fishio.Domain.Entities;

public class CompetitionFishCatch : BaseAuditableEntity
{
    public int CompetitionId { get; private set; }
    public virtual Competition Competition { get; private set; } = null!;

    public int ParticipantId { get; private set; }
    public virtual CompetitionParticipant Participant { get; private set; } = null!;

    public int JudgeId { get; private set; }
    public virtual User Judge { get; private set; } = null!;

    public int FishSpeciesId { get; private set; }
    public virtual FishSpecies FishSpecies { get; private set; } = null!;

    /// <summary>
    /// The length of the fish in centimeters.
    /// </summary>
    public decimal? Length { get; private set; }
    /// <summary>
    /// The weight of the fish in kilograms.
    /// </summary>
    public decimal? Weight { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;
    public DateTimeOffset CatchTime { get; private set; } = DateTimeOffset.UtcNow;

    private CompetitionFishCatch() { }

    public CompetitionFishCatch(
        Competition competition,
        CompetitionParticipant participant,
        User judge,
        int fishSpeciesId,
        string imageUrl,
        DateTimeOffset catchTime)
    {
        Competition = competition;
        CompetitionId = competition.Id;
        Participant = participant;
        ParticipantId = participant.Id;
        Judge = judge;
        JudgeId = judge.Id;
        FishSpeciesId = fishSpeciesId;
        ImageUrl = imageUrl;
        CatchTime = catchTime;
    }
}