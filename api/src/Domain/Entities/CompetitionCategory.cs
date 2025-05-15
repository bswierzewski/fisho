using Fishio.Domain.Common;

namespace Fishio.Domain.Entities;

public class CompetitionCategory : BaseAuditableEntity
{

    public int CompetitionId { get; set; }
    public Competition Competition { get; set; } = null!;

    public int CategoryDefinitionId { get; set; }
    public CategoryDefinition CategoryDefinition { get; set; } = null!;

    public int? SpecificFishSpeciesId { get; set; }
    public FishSpecies? SpecificFishSpecies { get; set; }

    public string? CustomNameOverride { get; set; }
    public string? CustomDescriptionOverride { get; set; }
    public int SortOrder { get; set; } = 0;
    public bool IsPrimaryScoring { get; set; } = false;
    public int MaxWinnersToDisplay { get; set; } = 1;
    public bool IsEnabled { get; set; } = true;
}
