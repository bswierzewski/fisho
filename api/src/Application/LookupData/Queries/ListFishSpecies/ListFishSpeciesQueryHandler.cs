namespace Fishio.Application.LookupData.Queries.ListFishSpecies;

public class ListFishSpeciesQueryHandler : IRequestHandler<ListFishSpeciesQuery, List<FishSpeciesDto>>
{
    private readonly IApplicationDbContext _context;

    public ListFishSpeciesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<FishSpeciesDto>> Handle(ListFishSpeciesQuery request, CancellationToken cancellationToken)
    {
        var fishSpeciesList = await _context.FishSpecies
                    .OrderBy(fs => fs.Name)
                    .Select(fs => new FishSpeciesDto(fs.Id, fs.Name))
                    .ToListAsync(cancellationToken);

        return fishSpeciesList;
    }
} 