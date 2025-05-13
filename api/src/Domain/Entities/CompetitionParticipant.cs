namespace Fishio.Domain.Entities;

// Plik: CompetitionParticipant.cs (Tabela łącząca dla uczestników i ich ról)
public class CompetitionParticipant : BaseAuditableEntity // Uczestnictwo też może być audytowalne
{
    public int CompetitionId { get; private set; }
    public virtual Competition Competition { get; private set; } = null!;

    public int? UserId { get; private set; } // Nullable for guest participants
    public virtual User? User { get; private set; }

    public string? GuestName { get; private set; } // For participants without an account
    public string? GuestIdentifier { get; private set; } // Optional unique ID for guest

    public ParticipantRole Role { get; private set; }
    public bool AddedByOrganizer { get; private set; } // True if manually added by organizer

    // Navigation property
    public virtual ICollection<CompetitionFishCatch> FishCatches { get; private set; } = new List<CompetitionFishCatch>();

    // Private constructor for EF Core
    private CompetitionParticipant() { }

    // Constructor for registered user
    public CompetitionParticipant(Competition competition, User user, ParticipantRole role, bool addedByOrganizer)
    {
        Competition = competition;
        CompetitionId = competition.Id;
        User = user;
        UserId = user.Id;
        Role = role;
        AddedByOrganizer = addedByOrganizer;
    }

    // Constructor for guest participant
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
