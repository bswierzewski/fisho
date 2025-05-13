namespace Fishio.Application.LookupData.Queries.ListScoringCategories;

public class ListScoringCategoriesQueryHandler : IRequestHandler<ListScoringCategoriesQuery, List<ScoringCategoryDto>>
{
    private readonly IApplicationDbContext _context;

    public ListScoringCategoriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ScoringCategoryDto>> Handle(ListScoringCategoriesQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for listing scoring categories
        return new List<ScoringCategoryDto>();
    }
} 