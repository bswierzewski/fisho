namespace Fishio.Application.LookupData.Queries.ListCategoryDefinitions;

public class ListCategoryDefinitionsQueryHandler : IRequestHandler<ListCategoryDefinitionsQuery, IEnumerable<CategoryDefinitionDto>>
{
    private readonly IApplicationDbContext _context;

    public ListCategoryDefinitionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryDefinitionDto>> Handle(ListCategoryDefinitionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.CategoryDefinitions
            .AsNoTracking()
            .Where(cd => cd.IsGlobal);

        if (request.FilterByType.HasValue)
        {
            query = query.Where(cd => cd.Type == request.FilterByType.Value);
        }

        return await query
            .OrderBy(cd => cd.Type)
            .ThenBy(cd => cd.Name)
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
                IsGlobal = cd.IsGlobal
            })
            .ToListAsync(cancellationToken);
    }
}
