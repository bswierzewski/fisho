namespace Fishio.Domain.Entities;

// Plik: User.cs (Podstawowe dane, reszta w Clerk)
public class User : BaseAuditableEntity
{
    public string ClerkUserId { get; private set; } = string.Empty; // Identyfikator z Clerk
    public string Name { get; private set; } = string.Empty;
    public string? Email { get; private set; }


    // Właściwości nawigacyjne
    public virtual ICollection<Competition> OrganizedCompetitions { get; private set; } = new List<Competition>();
    public virtual ICollection<CompetitionParticipant> CompetitionParticipations { get; private set; } = new List<CompetitionParticipant>();
    public virtual ICollection<CompetitionFishCatch> JudgedFishCatches { get; private set; } = new List<CompetitionFishCatch>();
    public virtual ICollection<LogbookEntry> LogbookEntries { get; private set; } = new List<LogbookEntry>();
    public virtual ICollection<Fishery> CreatedFisheries { get; private set; } = new List<Fishery>();

    // Private constructor for EF Core
    private User() { }

    public User(string clerkUserId, string name, string? email)
    {
        if (string.IsNullOrWhiteSpace(clerkUserId))
            throw new ArgumentNullException(nameof(clerkUserId));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name)); // Można ustawić domyślną, jeśli Clerk nie zawsze zwraca

        ClerkUserId = clerkUserId;
        Name = name;
        Email = email;
    }

    // Metoda do aktualizacji danych użytkownika na podstawie danych z Clerk
    public void UpdateDetailsFromClerk(string newName, string? newEmail)
    {
        if (Name != newName && !string.IsNullOrWhiteSpace(newName))
        {
            Name = newName;
        }

        if (Email != newEmail) // Pozwalamy na ustawienie emaila na null, jeśli tak przyjdzie z Clerk
        {
            Email = newEmail;
        }
    }
}
