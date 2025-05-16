namespace Fishio.Domain.Entities;

public class CategoryDefinition : BaseAuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsGlobal { get; set; } = true;
    public CategoryType Type { get; set; }
    public CategoryMetric Metric { get; set; }
    public CategoryCalculationLogic CalculationLogic { get; set; }
    public CategoryEntityType EntityType { get; set; }
    public bool RequiresSpecificFishSpecies { get; set; } = false;
    public bool AllowManualWinnerAssignment { get; set; } = true;

    // Prywatny konstruktor dla EF Core i dla HasData
    // HasData będzie musiało przypisać wartości do wszystkich właściwości.
    private CategoryDefinition() { }

}
