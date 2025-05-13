namespace Fishio.Application.Logbook.Queries.GetLogbookEntryDetails;

public class GetLogbookEntryDetailsQueryHandler : IRequestHandler<GetLogbookEntryDetailsQuery, LogbookEntryDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetLogbookEntryDetailsQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<LogbookEntryDetailsDto> Handle(GetLogbookEntryDetailsQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for getting logbook entry details
        return new LogbookEntryDetailsDto();
    }
} 