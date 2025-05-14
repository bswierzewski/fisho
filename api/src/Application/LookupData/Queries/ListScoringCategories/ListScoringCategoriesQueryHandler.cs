namespace Fishio.Application.LookupData.Queries.ListScoringCategories;

public class ListScoringCategoriesQueryHandler : IRequestHandler<ListScoringCategoriesQuery, List<ScoringCategoryOptionDto>>
{
    private readonly IApplicationDbContext _context;

    public ListScoringCategoriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ScoringCategoryOptionDto>> Handle(ListScoringCategoriesQuery request, CancellationToken cancellationToken)
    {
        var scoringCategories = await _context.ScoringCategoryOptions
                    .OrderBy(sco => sco.Name)
                    .Select(sco => new ScoringCategoryOptionDto(sco.Id, sco.Name, sco.Description))
                    .ToListAsync(cancellationToken);

        return scoringCategories;
    }
} 