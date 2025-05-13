namespace Fishio.Application.Logbook.Queries.ListLogbookEntries;

public class ListLogbookEntriesQueryHandler : IRequestHandler<ListLogbookEntriesQuery, List<LogbookEntryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ListLogbookEntriesQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<LogbookEntryDto>> Handle(ListLogbookEntriesQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for listing logbook entries
        return new List<LogbookEntryDto>();
    }
} 