namespace Fishio.Application.LookupData.Queries.ListScoringCategories;

public record ListScoringCategoriesQuery : IRequest<List<ScoringCategoryDto>>;

public record ScoringCategoryDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public List<ScoringCategoryOptionDto> Options { get; init; } = new();
}

public record ScoringCategoryOptionDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int Points { get; init; }
} 