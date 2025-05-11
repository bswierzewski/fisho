namespace Domain.Entities;

// Plik: CompetitionParticipant.cs (Tabela łącząca dla uczestników i ich ról)
public class CompetitionParticipant : BaseAuditableEntity // Uczestnictwo też może być audytowalne
{
    // Id, Created, CreatedBy, LastModified, LastModifiedBy dziedziczone
    public int CompetitionId { get; set; }
    public int? UserId { get; set; }
    public string? GuestName { get; set; }
    public string? GuestIdentifier { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool AddedByOrganizer { get; set; }

    // Właściwości nawigacyjne
    public virtual Competition Competition { get; set; } = null!;
    public virtual User? User { get; set; }
    public virtual ICollection<CompetitionFishCatch> FishCatches { get; set; } = new List<CompetitionFishCatch>();
}
