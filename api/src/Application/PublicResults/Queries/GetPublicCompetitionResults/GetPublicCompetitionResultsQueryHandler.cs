namespace Fishio.Application.PublicResults.Queries.GetPublicCompetitionResults;

public class GetPublicCompetitionResultsQueryHandler : IRequestHandler<GetPublicCompetitionResultsQuery, PublicCompetitionResultsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICompetitionTokenGenerator _tokenGenerator;

    public GetPublicCompetitionResultsQueryHandler(
        IApplicationDbContext context,
        ICompetitionTokenGenerator tokenGenerator)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<PublicCompetitionResultsDto> Handle(GetPublicCompetitionResultsQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for getting public competition results
        return new PublicCompetitionResultsDto();
    }
} 