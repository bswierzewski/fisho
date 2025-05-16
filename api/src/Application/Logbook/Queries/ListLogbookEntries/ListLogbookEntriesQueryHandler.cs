using Fishio.Application.Common.Mappings;

namespace Fishio.Application.Logbook.Queries.ListLogbookEntries;

public class ListLogbookEntriesQueryHandler : IRequestHandler<ListLogbookEntriesQuery, PaginatedList<LogbookEntryDto>>
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

    public async Task<PaginatedList<LogbookEntryDto>> Handle(ListLogbookEntriesQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.DomainUserId;
        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException("Użytkownik musi być zalogowany, aby wyświetlić dziennik.");
        }

        var query = _context.LogbookEntries
            .AsNoTracking()
            .Where(le => le.UserId == userId.Value);

        if (request.FishSpeciesId.HasValue)
        {
            query = query.Where(le => le.FishSpeciesId == request.FishSpeciesId.Value);
        }
        if (request.FromDate.HasValue)
        {
            query = query.Where(le => le.CatchTime >= request.FromDate.Value);
        }
        if (request.ToDate.HasValue)
        {
            // Dodaj jeden dzień do FilterByDateTo, aby uwzględnić cały dzień
            var dateTo = request.ToDate.Value.Date.AddDays(1);
            query = query.Where(le => le.CatchTime < dateTo);
        }
        if (request.FisheryId.HasValue)
        {
            query = query.Where(le => le.FisheryId == request.FisheryId.Value);
        }

        // Ręczne mapowanie i paginacja
        var logbookEntriesQuery = query
            .Include(le => le.Fishery) // Dla FisheryName
            .OrderByDescending(le => le.CatchTime)
            .Select(le => new LogbookEntryDto
            {
                Id = le.Id,
                FishSpeciesName = le.FishSpecies != null ? le.FishSpecies.Name : null,
                CaughtAt = le.CatchTime,
                Length = le.Length,
                Weight = le.Weight,
                PhotoUrl = le.PhotoUrl,
                FisheryName = le.Fishery != null ? le.Fishery.Name : null
            });

        return await logbookEntriesQuery.ToPaginatedListAsync(request.Page, request.PageSize);
    }
}