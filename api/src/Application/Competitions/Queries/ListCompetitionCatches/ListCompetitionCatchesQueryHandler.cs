namespace Fishio.Application.Competitions.Queries.ListCompetitionCatches;

public class ListCompetitionCatchesQueryHandler : IRequestHandler<ListCompetitionCatchesQuery, List<CompetitionCatchDto>>
{
    private readonly IApplicationDbContext _context;

    public ListCompetitionCatchesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CompetitionCatchDto>> Handle(ListCompetitionCatchesQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for listing competition catches
        return new List<CompetitionCatchDto>();
    }
} 