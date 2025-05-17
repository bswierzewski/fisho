namespace Fishio.Application.Competitions.Queries.ListCompetitions;

public class ListCompetitionsQueryHandler : IRequestHandler<ListCompetitionsQuery, List<CompetitionDto>>
{
    private readonly IApplicationDbContext _context;

    public ListCompetitionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CompetitionDto>> Handle(ListCompetitionsQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for listing competitions
        return await Task.FromResult(new List<CompetitionDto>());
    }
}