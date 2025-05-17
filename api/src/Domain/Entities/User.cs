namespace Fishio.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string ClerkUserId { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Email { get; private set; }

    public virtual ICollection<Competition> OrganizedCompetitions { get; private set; } = [];
    public virtual ICollection<CompetitionParticipant> CompetitionParticipations { get; private set; } = [];
    public virtual ICollection<CompetitionFishCatch> JudgedFishCatches { get; private set; } = [];
    public virtual ICollection<LogbookEntry> LogbookEntries { get; private set; } = [];
    public virtual ICollection<Fishery> CreatedFisheries { get; private set; } = [];

    private User() { }

    public User(string clerkUserId, string name, string? email)
    {
        Guard.Against.NullOrWhiteSpace(clerkUserId, nameof(clerkUserId));
        Guard.Against.NullOrWhiteSpace(name, nameof(name));

        ClerkUserId = clerkUserId;
        Name = name;
        Email = email;
    }

    public void UpdateDetailsFromClerk(string newName, string? newEmail)
    {
        if (Name != newName && !string.IsNullOrWhiteSpace(newName))
            Name = newName;

        Email = newEmail;
    }
}
