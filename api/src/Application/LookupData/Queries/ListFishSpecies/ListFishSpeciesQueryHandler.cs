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
        // TODO: Implement the logic for listing fish species
        return new List<FishSpeciesDto>();
    }
} 