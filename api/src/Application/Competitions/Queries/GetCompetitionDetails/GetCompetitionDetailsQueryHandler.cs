namespace Fishio.Application.Competitions.Queries.GetCompetitionDetails;

public class GetCompetitionDetailsQueryHandler : IRequestHandler<GetCompetitionDetailsQuery, CompetitionDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetCompetitionDetailsQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CompetitionDetailsDto> Handle(GetCompetitionDetailsQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for getting competition details
        return new CompetitionDetailsDto();
    }
}
