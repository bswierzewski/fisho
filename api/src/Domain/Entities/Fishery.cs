namespace Fishio.Domain.Entities;

public class Fishery : BaseAuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? ImageUrl { get; private set; }
    public string? Location { get; private set; }

    /// <summary>
    /// Creator/Maintainer of this fishery entry
    /// </summary>
    public int? UserId { get; private set; }
    public virtual User? User { get; private set; }

    public virtual ICollection<LogbookEntry> LogbookEntries { get; private set; } = [];
    public virtual ICollection<FishSpecies> FishSpecies { get; private set; } = [];
    public virtual ICollection<Competition> Competitions { get; private set; } = [];

    private Fishery() { }

    public Fishery(int userId, string name, string? imageUrl, string? location)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name), "Nazwa łowiska jest wymagana.");

        Name = name;
        UserId = userId;
        Location = location;
        ImageUrl = imageUrl;
    }

    public void UpdateDetails(string name, string? imageUrl, string? location)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Name = name;

        Location = location;
        ImageUrl = imageUrl;
    }

    public void AddSpecies(FishSpecies species)
    {
        if (!FishSpecies.Contains(species))
            FishSpecies.Add(species);
    }

    public void RemoveSpecies(FishSpecies species)
    {
        FishSpecies.Remove(species);
    }
}
