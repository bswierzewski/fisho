namespace Fishio.Application.LookupData.Queries.GetCategoryDefinitions;

public class GetCategoryDefinitionsQuery : IRequest<IEnumerable<CategoryDefinitionDto>>
{
    // Można dodać parametry filtrowania, np. po CategoryType, jeśli potrzebne
    public CategoryType? FilterByType { get; set; }
}

public class CategoryDefinitionDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public CategoryType Type { get; init; }
    public CategoryMetric Metric { get; init; }
    public CategoryCalculationLogic CalculationLogic { get; init; }
    public CategoryEntityType EntityType { get; init; }
    public bool RequiresSpecificFishSpecies { get; init; }
    public bool AllowManualWinnerAssignment { get; init; }
}
