using Fishio.Application.Logbook.Queries.ListLogbookEntries;

namespace Fishio.Application.LogbookEntries.Queries.GetUserLogbookEntries;

public class GetUserLogbookEntriesQueryHandler : IRequestHandler<GetUserLogbookEntriesQuery, PaginatedList<UserLogbookEntryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetUserLogbookEntriesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<UserLogbookEntryDto>> Handle(GetUserLogbookEntriesQuery request, CancellationToken cancellationToken)
    {
        var domainUserId = _currentUserService.UserId;
        if (!domainUserId.HasValue || domainUserId.Value == 0)
        {
            throw new UnauthorizedAccessException("Użytkownik nie jest zidentyfikowany.");
        }

        IQueryable<LogbookEntry> query = _context.LogbookEntries
            .AsNoTracking()
            .Where(le => le.UserId == domainUserId.Value)
            .Include(le => le.FishSpecies) // Do pobrania FishSpeciesName
            .Include(le => le.Fishery);    // Do pobrania FisheryName

        query = query.OrderByDescending(le => le.CatchTime); // Najnowsze najpierw

        var paginatedEntries = await PaginatedList<LogbookEntry>.CreateAsync(query, request.PageNumber, request.PageSize);

        var entryDtos = paginatedEntries.Items.Select(le => new UserLogbookEntryDto
        {
            Id = le.Id,
            ImageUrl = le.ImageUrl,
            CatchTime = le.CatchTime,
            LengthInCm = le.Length?.Value, // Dostęp do wartości z ValueObject
            WeightInKg = le.Weight?.Value, // Dostęp do wartości z ValueObject
            Notes = le.Notes,
            FishSpeciesId = le.FishSpeciesId,
            FishSpeciesName = le.FishSpecies?.Name,
            FisheryId = le.FisheryId,
            FisheryName = le.Fishery?.Name,
            Created = le.Created
        }).ToList();

        return new PaginatedList<UserLogbookEntryDto>(entryDtos, paginatedEntries.TotalCount, paginatedEntries.PageNumber, request.PageSize);
    }
}
