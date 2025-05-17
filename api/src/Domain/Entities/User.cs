namespace Fishio.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string ClerkUserId { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? ImageUrl { get; private set; }

    public virtual ICollection<Competition> OrganizedCompetitions { get; private set; } = [];
    public virtual ICollection<CompetitionParticipant> CompetitionParticipations { get; private set; } = [];
    public virtual ICollection<CompetitionFishCatch> JudgedFishCatches { get; private set; } = [];
    public virtual ICollection<LogbookEntry> LogbookEntries { get; private set; } = [];
    public virtual ICollection<Fishery> CreatedFisheries { get; private set; } = [];

    private User() { }

    public User(string clerkUserId, string name, string? email, string? imageUrl)
    {
        Guard.Against.NullOrWhiteSpace(clerkUserId, nameof(clerkUserId));
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        // Można dodać walidację formatu email, jeśli jest wymagany

        ClerkUserId = clerkUserId;
        Name = name;
        Email = email;
        ImageUrl = imageUrl;
    }

    /// <summary>
    /// Aktualizuje dane użytkownika na podstawie informacji z systemu Clerk.
    /// </summary>
    public void UpdateDetailsFromClerk(string newName, string? newEmail, string? newImageUrl)
    {
        Guard.Against.NullOrWhiteSpace(newName, nameof(newName));

        if (Name != newName)
        {
            Name = newName;
        }
        // Email i ImageUrl mogą być null, więc pozwalamy na ich aktualizację na null
        Email = newEmail;
        ImageUrl = newImageUrl;

        // Można dodać zdarzenie domenowe: UserDetailsUpdatedFromClerkEvent(this)
    }

    // Potencjalne przyszłe metody domenowe dla User:
    // - UpdatePreferences (jeśli użytkownik miałby jakieś preferencje przechowywane w aplikacji)

    // Metody sprawdzające, bardziej jako helpery niż czysta logika domenowa,
    // ale mogą być użyteczne w warstwie aplikacji:
    public bool IsOrganizerOf(Competition competition)
    {
        Guard.Against.Null(competition, nameof(competition));
        return competition.OrganizerId == Id;
    }

    public bool IsParticipantIn(Competition competition)
    {
        Guard.Against.Null(competition, nameof(competition));
        return CompetitionParticipations.Any(cp => cp.CompetitionId == competition.Id);
    }

    public bool IsJudgeIn(Competition competition)
    {
        Guard.Against.Null(competition, nameof(competition));
        return CompetitionParticipations.Any(cp => cp.CompetitionId == competition.Id && cp.Role == ParticipantRole.Judge);
    }
}
