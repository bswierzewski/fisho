namespace Fishio.Application.Competitions.Queries.ListCompetitionParticipants;

public class ListCompetitionParticipantsQueryHandler : IRequestHandler<ListCompetitionParticipantsQuery, List<CompetitionParticipantDto>>
{
    private readonly IApplicationDbContext _context;

    public ListCompetitionParticipantsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CompetitionParticipantDto>> Handle(ListCompetitionParticipantsQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for listing competition participants
        return new List<CompetitionParticipantDto>();
    }
} 