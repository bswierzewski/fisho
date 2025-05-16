namespace Fishio.Domain.Entities;

public class FishSpecies : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    // Navigation property for many-to-many with Fishery
    public virtual ICollection<Fishery> Fisheries { get; private set; } = [];

    // Private constructor for EF Core
    private FishSpecies() { }

    public FishSpecies(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Name = name;
    }
}
