namespace Fishio.Application.LogbookEntries.Queries.GetLogbookEntryById;

public class GetLogbookEntryByIdQueryHandler : IRequestHandler<GetLogbookEntryByIdQuery, LogbookEntryDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetLogbookEntryByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<LogbookEntryDto?> Handle(GetLogbookEntryByIdQuery request, CancellationToken cancellationToken)
    {
        var domainUserId = _currentUserService.UserId;
        if (!domainUserId.HasValue || domainUserId.Value == 0)
        {
            throw new UnauthorizedAccessException("Użytkownik nie jest zidentyfikowany.");
        }

        var logbookEntry = await _context.LogbookEntries
            .AsNoTracking()
            .Where(le => le.Id == request.Id)
            .Include(le => le.FishSpecies)
            .Include(le => le.Fishery)
            .FirstOrDefaultAsync(cancellationToken);

        if (logbookEntry == null)
        {
            return null; // Lub rzucić NotFoundException, jeśli preferowane dla GET by ID
            // throw new NotFoundException(nameof(LogbookEntry), request.Id);
        }

        // Sprawdzenie, czy użytkownik ma dostęp do tego konkretnego wpisu
        if (logbookEntry.UserId != domainUserId.Value)
        {
            // Zamiast rzucać ForbiddenAccessException, można po prostu zwrócić null,
            // tak jakby wpis nie istniał dla tego użytkownika.
            // To zależy od preferencji bezpieczeństwa (czy ujawniać istnienie zasobu).
            return null;
            // throw new ForbiddenAccessException("Nie masz uprawnień do wyświetlenia tego wpisu.");
        }

        return new LogbookEntryDto
        {
            Id = logbookEntry.Id,
            ImageUrl = logbookEntry.ImageUrl,
            CatchTime = logbookEntry.CatchTime,
            LengthInCm = logbookEntry.Length?.Value,
            WeightInKg = logbookEntry.Weight?.Value,
            Notes = logbookEntry.Notes,
            FishSpeciesId = logbookEntry.FishSpeciesId,
            FishSpeciesName = logbookEntry.FishSpecies?.Name,
            FisheryId = logbookEntry.FisheryId,
            FisheryName = logbookEntry.Fishery?.Name,
            Created = logbookEntry.Created
        };
    }
}
