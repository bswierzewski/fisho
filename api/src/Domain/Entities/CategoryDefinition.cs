public class CategoryDefinition : BaseAuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsGlobal { get; private set; }
    public CategoryType Type { get; private set; }
    public CategoryMetric Metric { get; private set; }
    public CategoryCalculationLogic CalculationLogic { get; private set; }
    public CategoryEntityType EntityType { get; private set; }
    public bool RequiresSpecificFishSpecies { get; private set; }
    public bool AllowManualWinnerAssignment { get; private set; }

    // Prywatny konstruktor dla EF Core do materializacji obiektów z bazy
    private CategoryDefinition() { }

    // Publiczny konstruktor dla HasData i potencjalnie dla testów
    public CategoryDefinition(
        string name,
        string? description,
        bool isGlobal,
        CategoryType type,
        CategoryMetric metric,
        CategoryCalculationLogic calculationLogic,
        CategoryEntityType entityType,
        bool requiresSpecificFishSpecies,
        bool allowManualWinnerAssignment
        )
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));

        Name = name;
        Description = description;
        IsGlobal = isGlobal;
        Type = type;
        Metric = metric;
        CalculationLogic = calculationLogic;
        EntityType = entityType;
        RequiresSpecificFishSpecies = requiresSpecificFishSpecies;
        AllowManualWinnerAssignment = allowManualWinnerAssignment;
    }
}