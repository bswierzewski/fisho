namespace Fishio.Domain.Entities;

public class CompetitionCategory : BaseAuditableEntity
{

    public int CompetitionId { get; private set; }
    public Competition Competition { get; private set; } = null!;

    public int CategoryDefinitionId { get; private set; }
    public CategoryDefinition CategoryDefinition { get; private set; } = null!;

    public int? SpecificFishSpeciesId { get; private set; }
    public FishSpecies? SpecificFishSpecies { get; private set; }

    public string? CustomNameOverride { get; private set; }
    public string? CustomDescriptionOverride { get; private set; }
    public int SortOrder { get; private set; } = 0;
    public bool IsPrimaryScoring { get; private set; } = false;
    public int MaxWinnersToDisplay { get; private set; } = 1;
    public bool IsEnabled { get; private set; } = true;

    // Prywatny konstruktor dla EF Core
    private CompetitionCategory() { }

    // Konstruktor wywoływany przez Competition.AddCategory
    internal CompetitionCategory( // internal, aby tylko Competition mogło tworzyć
        int competitionId,
        int categoryDefinitionId,
        int? specificFishSpeciesId,
        string? customNameOverride,
        string? customDescriptionOverride,
        int sortOrder,
        bool isPrimaryScoring,
        int maxWinnersToDisplay,
        bool isEnabled)
    {
        // Podstawowa walidacja może być tutaj, ale główna logika biznesowa w Competition
        Guard.Against.NegativeOrZero(competitionId, nameof(competitionId));
        Guard.Against.NegativeOrZero(categoryDefinitionId, nameof(categoryDefinitionId));
        Guard.Against.NegativeOrZero(maxWinnersToDisplay, nameof(maxWinnersToDisplay));

        CompetitionId = competitionId;
        CategoryDefinitionId = categoryDefinitionId;
        SpecificFishSpeciesId = specificFishSpeciesId;
        CustomNameOverride = customNameOverride;
        CustomDescriptionOverride = customDescriptionOverride;
        SortOrder = sortOrder;
        IsPrimaryScoring = isPrimaryScoring;
        MaxWinnersToDisplay = maxWinnersToDisplay;
        IsEnabled = isEnabled;
    }
}
