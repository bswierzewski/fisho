namespace Fishio.Domain.Entities;

public class FishSpecies : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public virtual ICollection<Fishery> Fisheries { get; private set; } = [];

    // Private constructor for EF Core
    private FishSpecies() { }

    public FishSpecies(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name), "Nazwa łowiska jest wymagana.");
        Guard.Against.LengthOutOfRange(name, 1, 255, "Nazwa łowiska musi mieć od 1 do 255 znaków.");
        Name = name;
    }
}
