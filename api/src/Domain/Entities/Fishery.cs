namespace Fishio.Domain.Entities;

public class Fishery : BaseAuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Location { get; private set; }
    public string? ImageUrl { get; private set; }

    public int? UserId { get; private set; } // Creator/Maintainer of this fishery entry, nullable
    public virtual User? User { get; private set; }

    // Navigation properties
    public virtual ICollection<LogbookEntry> LogbookEntries { get; private set; } = new List<LogbookEntry>();
    public virtual ICollection<FishSpecies> DefinedSpecies { get; private set; } = new List<FishSpecies>(); // Many-to-many

    // Private constructor for EF Core
    private Fishery() { }

    public Fishery(string name, int userId, string location, string? imageUrl)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name), "Nazwa łowiska jest wymagana.");
        Name = name;

        UserId = userId;

        Location = location;

        ImageUrl = imageUrl;
    }

    public void UpdateDetails(string? name, string? location, string? imageUrl)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Name = name!;

        Guard.Against.NullOrWhiteSpace(location, nameof(location));
        Location = location;

        Guard.Against.NullOrWhiteSpace(imageUrl, nameof(imageUrl));
        ImageUrl = imageUrl;
    }

    public void AddSpecies(FishSpecies species)
    {
        if (!DefinedSpecies.Contains(species))
        {
            DefinedSpecies.Add(species);
        }
    }

    public void RemoveSpecies(FishSpecies species)
    {
        DefinedSpecies.Remove(species);
    }
}
