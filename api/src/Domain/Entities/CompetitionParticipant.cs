namespace Fishio.Domain.Entities;

/// <summary>
/// Represents a participant in a competition.
/// This can be a user or a guest.
/// The participant can have different roles such as competitor, judge, or organizer.
/// The participant can be added by the organizer or join on their own.
/// </summary>
public class CompetitionParticipant : BaseAuditableEntity
{
    public int CompetitionId { get; private set; }
    public virtual Competition Competition { get; private set; } = null!;

    public int? UserId { get; private set; }
    public virtual User? User { get; private set; }

    public string? GuestName { get; private set; }
    public string? GuestIdentifier { get; private set; }

    public ParticipantRole Role { get; private set; }
    public bool AddedByOrganizer { get; private set; }

    public virtual ICollection<CompetitionFishCatch> FishCatches { get; private set; } = [];

    private CompetitionParticipant() { }

    public CompetitionParticipant(Competition competition, User user, ParticipantRole role, bool addedByOrganizer)
    {
        Competition = competition;
        CompetitionId = competition.Id;
        User = user;
        UserId = user.Id;
        Role = role;
        AddedByOrganizer = addedByOrganizer;
    }

    public CompetitionParticipant(Competition competition, string guestName, ParticipantRole role, bool addedByOrganizer, string? guestIdentifier = null)
    {
        Competition = competition;
        CompetitionId = competition.Id;
        GuestName = guestName;
        GuestIdentifier = guestIdentifier;
        Role = role;
        AddedByOrganizer = addedByOrganizer;
    }
}
