namespace Fishio.Application.LookupData.Queries.ListCategoryDefinitions;

public record ListCategoryDefinitionsQuery(CategoryType? FilterByType = null) : IRequest<IEnumerable<CategoryDefinitionDto>>;

public class CategoryDefinitionDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public CategoryType Type { get; set; }
    public CategoryMetric Metric { get; set; }
    public CategoryCalculationLogic CalculationLogic { get; set; }
    public CategoryEntityType EntityType { get; set; }
    public bool RequiresSpecificFishSpecies { get; set; }
    public bool IsGlobal { get; set; }
}
