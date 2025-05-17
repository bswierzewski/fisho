namespace Fishio.Domain.Entities;

public class CompetitionCategory : BaseAuditableEntity
{

    public int CompetitionId { get; private set; }
    public Competition Competition { get; private set; } = null!;

    public int CategoryDefinitionId { get; private set; }
    public CategoryDefinition CategoryDefinition { get; private set; } = null!;

    public int? FishSpeciesId { get; private set; }
    public FishSpecies? FishSpecies { get; private set; }

    public string? CustomNameOverride { get; private set; }
    public string? CustomDescriptionOverride { get; private set; }
    public int SortOrder { get; private set; } = 0;
    public bool IsPrimaryScoring { get; private set; } = false;
    public int MaxWinnersToDisplay { get; private set; } = 1;
    public bool IsEnabled { get; private set; } = true;

    private CompetitionCategory() { }

    // Konstruktor wewnętrzny, tworzenie przez Competition.AddCategory
    internal CompetitionCategory(
        Competition competition,
        CategoryDefinition categoryDefinition,
        bool isPrimaryScoring,
        int sortOrder = 0,
        int maxWinnersToDisplay = 1,
        int? fishSpeciesId = null,
        string? customNameOverride = null,
        string? customDescriptionOverride = null)
    {
        Guard.Against.Null(competition, nameof(competition));
        Guard.Against.Null(categoryDefinition, nameof(categoryDefinition));
        Guard.Against.NegativeOrZero(maxWinnersToDisplay, nameof(maxWinnersToDisplay));
        if (fishSpeciesId.HasValue) Guard.Against.NegativeOrZero(fishSpeciesId.Value, nameof(fishSpeciesId));

        if (categoryDefinition.RequiresSpecificFishSpecies && !fishSpeciesId.HasValue)
        {
            throw new ArgumentException($"Kategoria '{categoryDefinition.Name}' wymaga zdefiniowania konkretnego gatunku ryby.", nameof(fishSpeciesId));
        }
        if (!categoryDefinition.RequiresSpecificFishSpecies && fishSpeciesId.HasValue)
        {
            throw new ArgumentException($"Kategoria '{categoryDefinition.Name}' nie powinna mieć zdefiniowanego gatunku ryby.", nameof(fishSpeciesId));
        }


        CompetitionId = competition.Id;
        Competition = competition;
        CategoryDefinitionId = categoryDefinition.Id;
        CategoryDefinition = categoryDefinition;
        IsPrimaryScoring = isPrimaryScoring;
        SortOrder = sortOrder;
        MaxWinnersToDisplay = maxWinnersToDisplay;
        FishSpeciesId = fishSpeciesId;
        CustomNameOverride = customNameOverride;
        CustomDescriptionOverride = customDescriptionOverride;
        IsEnabled = true;
    }

    public void UpdateConfiguration(
        bool isPrimaryScoring,
        int sortOrder,
        int maxWinnersToDisplay,
        int? fishSpeciesId,
        string? customNameOverride,
        string? customDescriptionOverride)
    {
        // TODO: Dodać walidację, czy można modyfikować (np. zawody nie trwają/zakończone)
        Guard.Against.NegativeOrZero(maxWinnersToDisplay, nameof(maxWinnersToDisplay));
        if (fishSpeciesId.HasValue) Guard.Against.NegativeOrZero(fishSpeciesId.Value, nameof(fishSpeciesId));

        if (CategoryDefinition.RequiresSpecificFishSpecies && !fishSpeciesId.HasValue)
        {
            throw new ArgumentException($"Kategoria '{CategoryDefinition.Name}' wymaga zdefiniowania konkretnego gatunku ryby.", nameof(fishSpeciesId));
        }
        if (!CategoryDefinition.RequiresSpecificFishSpecies && fishSpeciesId.HasValue)
        {
            throw new ArgumentException($"Kategoria '{CategoryDefinition.Name}' nie powinna mieć zdefiniowanego gatunku ryby.", nameof(fishSpeciesId));
        }

        IsPrimaryScoring = isPrimaryScoring;
        SortOrder = sortOrder;
        MaxWinnersToDisplay = maxWinnersToDisplay;
        FishSpeciesId = fishSpeciesId;
        CustomNameOverride = customNameOverride;
        CustomDescriptionOverride = customDescriptionOverride;
    }

    public void Enable() => IsEnabled = true;
    public void Disable() => IsEnabled = false;
}
