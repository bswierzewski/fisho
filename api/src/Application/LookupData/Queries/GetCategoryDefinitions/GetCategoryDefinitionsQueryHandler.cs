namespace Fishio.Application.LookupData.Queries.GetCategoryDefinitions;

public class GetCategoryDefinitionsQueryHandler : IRequestHandler<GetCategoryDefinitionsQuery, IEnumerable<CategoryDefinitionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryDefinitionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryDefinitionDto>> Handle(GetCategoryDefinitionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.CategoryDefinitions
            .AsNoTracking()
            .Where(cd => cd.IsGlobal); // Pobieramy tylko globalne definicje

        // Przykład dodatkowego filtrowania, jeśli byłby parametr w query
        if (request.FilterByType.HasValue)
        {
            query = query.Where(cd => cd.Type == request.FilterByType.Value);
        }

        var categoryDefinitions = await query
            .OrderBy(cd => cd.Name) // Sortowanie alfabetyczne
            .Select(cd => new CategoryDefinitionDto
            {
                Id = cd.Id,
                Name = cd.Name,
                Description = cd.Description,
                Type = cd.Type,
                Metric = cd.Metric,
                CalculationLogic = cd.CalculationLogic,
                EntityType = cd.EntityType,
                RequiresSpecificFishSpecies = cd.RequiresSpecificFishSpecies,
                AllowManualWinnerAssignment = cd.AllowManualWinnerAssignment
            })
            .ToListAsync(cancellationToken);

        return categoryDefinitions;
    }
}
