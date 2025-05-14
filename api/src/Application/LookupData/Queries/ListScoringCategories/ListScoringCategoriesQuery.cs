namespace Fishio.Application.LookupData.Queries.ListScoringCategories;

public record ListScoringCategoriesQuery : IRequest<List<ScoringCategoryOptionDto>>;

public record ScoringCategoryOptionDto(int Id, string Name, string? Description);