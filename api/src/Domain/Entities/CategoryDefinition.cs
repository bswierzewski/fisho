using Fishio.Domain.Common;
using Fishio.Domain.Enums;

namespace Fishio.Domain.Entities;

public class CategoryDefinition : BaseAuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsGlobal { get; set; } = true; // Domyślnie globalna
    public CategoryType Type { get; set; }
    public CategoryMetric Metric { get; set; }
    public CategoryCalculationLogic CalculationLogic { get; set; }
    public CategoryEntityType EntityType { get; set; }
    public bool RequiresSpecificFishSpecies { get; set; } = false;
    public bool AllowManualWinnerAssignment { get; set; } = true;

    // Relacja zwrotna do kategorii w zawodach
    public ICollection<CompetitionCategory> CompetitionCategories { get; private set; } =
        new List<CompetitionCategory>();
}
