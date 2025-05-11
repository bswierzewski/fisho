using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

// Plik: CompetitionFishCatch.cs
public class CompetitionFishCatch : BaseAuditableEntity // Zgłoszenie połowu też audytowalne
{
    // Id, Created, CreatedBy, LastModified, LastModifiedBy dziedziczone
    public int CompetitionId { get; set; }
    public int ParticipantId { get; set; }
    public int JudgeId { get; set; } // Powinno być mapowane na CreatedBy, jeśli sędzia to twórca zgłoszenia
    public string SpeciesName { get; set; } = string.Empty;
    public decimal? LengthCm { get; set; }
    public decimal? WeightKg { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public DateTimeOffset CatchTime { get; set; } // Zmieniono na DateTimeOffset

    // Właściwości nawigacyjne
    public virtual Competition Competition { get; set; } = null!;
    public virtual CompetitionParticipant Participant { get; set; } = null!;
    public virtual User Judge { get; set; } = null!;
}