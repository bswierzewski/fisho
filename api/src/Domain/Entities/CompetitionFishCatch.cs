using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

// Plik: CompetitionFishCatch.cs
public class CompetitionFishCatch : BaseAuditableEntity // Zgłoszenie połowu też audytowalne
{
    public int CompetitionId { get; private set; }
    public virtual Competition Competition { get; private set; } = null!;

    public int ParticipantId { get; private set; }
    public virtual CompetitionParticipant Participant { get; private set; } = null!;

    public int JudgeId { get; private set; }
    public virtual User Judge { get; private set; } = null!;

    public string SpeciesName { get; private set; } = string.Empty; // Could link to FishSpecies if catches are always predefined species
    public decimal? LengthCm { get; private set; }
    public decimal? WeightKg { get; private set; }
    public string PhotoUrl { get; private set; } = string.Empty;
    public DateTimeOffset CatchTime { get; private set; }

    // Private constructor for EF Core
    private CompetitionFishCatch() { }

    public CompetitionFishCatch(
        Competition competition,
        CompetitionParticipant participant,
        User judge,
        string speciesName,
        string photoUrl,
        DateTimeOffset catchTime)
    {
        Competition = competition;
        CompetitionId = competition.Id;
        Participant = participant;
        ParticipantId = participant.Id;
        Judge = judge;
        JudgeId = judge.Id;
        SpeciesName = speciesName;
        PhotoUrl = photoUrl;
        CatchTime = catchTime;
    }
}