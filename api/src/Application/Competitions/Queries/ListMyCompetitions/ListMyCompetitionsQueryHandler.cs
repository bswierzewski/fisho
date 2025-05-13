namespace Fishio.Application.Competitions.Queries.ListMyCompetitions;

public class ListMyCompetitionsQueryHandler : IRequestHandler<ListMyCompetitionsQuery, List<MyCompetitionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ListMyCompetitionsQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<MyCompetitionDto>> Handle(ListMyCompetitionsQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for listing user's competitions
        return new List<MyCompetitionDto>();
    }
} 