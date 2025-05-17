namespace Fishio.Domain.Entities;

public class FishSpecies : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    // Kolekcje nawigacyjne dla EF Core
    // Te kolekcje pokazują, gdzie dany gatunek jest używany,
    // ale niekoniecznie są zarządzane bezpośrednio z poziomu FishSpecies.
    public virtual ICollection<Fishery> Fisheries { get; private set; } = [];
    public virtual ICollection<LogbookEntry> LogbookEntries { get; private set; } = [];
    public virtual ICollection<CompetitionFishCatch> CompetitionFishCatches { get; private set; } = [];
    public virtual ICollection<CompetitionCategory> CompetitionCategories { get; private set; } = [];


    private FishSpecies() { } // Dla EF Core

    public FishSpecies(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name), "Nazwa gatunku ryby jest wymagana.");
        Guard.Against.LengthOutOfRange(name, 1, 100, nameof(name), "Nazwa gatunku musi mieć od 1 do 100 znaków."); // Skrócono z 255 dla przykładu
        Name = name.Trim(); // Upewnij się, że nie ma białych znaków na początku/końcu
    }

    public void UpdateName(string newName /*, User modifyingUser - do walidacji uprawnień, np. admin */)
    {
        // TODO: Dodać walidację uprawnień (np. tylko administrator może zmieniać nazwy gatunków)
        Guard.Against.NullOrWhiteSpace(newName, nameof(newName), "Nowa nazwa gatunku ryby jest wymagana.");
        Guard.Against.LengthOutOfRange(newName, 1, 100, nameof(newName), "Nowa nazwa gatunku musi mieć od 1 do 100 znaków.");

        if (Name != newName.Trim())
        {
            Name = newName.Trim();
            // AddDomainEvent(new FishSpeciesNameUpdatedEvent(this));
        }
    }
}
