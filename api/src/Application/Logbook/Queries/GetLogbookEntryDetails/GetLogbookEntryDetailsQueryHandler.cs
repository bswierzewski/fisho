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
        var logbookEntry = await _context.LogbookEntries
                    .AsNoTracking()
                    .Include(le => le.Fishery) // Dołączamy łowisko, aby uzyskać jego nazwę
                    .FirstOrDefaultAsync(le => le.Id == request.Id, cancellationToken);

        if (logbookEntry == null)
        {
            throw new NotFoundException(nameof(LogbookEntry), request.Id.ToString());
        }

        var currentUserId = _currentUserService.UserId;
        if (logbookEntry.UserId != currentUserId)
        {
            // Zamiast rzucać Forbidden, można też rzucić NotFound, aby nie ujawniać istnienia wpisu
            throw new NotFoundException(nameof(LogbookEntry), request.Id.ToString());
            // throw new ForbiddenAccessException("Nie masz uprawnień do wyświetlenia tego wpisu w dzienniku.");
        }

        // Ręczne mapowanie (lub użyj AutoMappera)
        return new LogbookEntryDetailsDto
        {
            Id = logbookEntry.Id,
            FishSpeciesName = logbookEntry.FishSpecies?.Name ?? "",
            Length = logbookEntry.Length,
            Weight = logbookEntry.Weight,
            ImageUrl = logbookEntry.ImageUrl,
            CaughtAt = logbookEntry.CatchTime,
            Notes = logbookEntry.Notes,
            FisheryId = logbookEntry.FisheryId,
            FisheryName = logbookEntry.Fishery?.Name ?? "",
            CreatedAt = logbookEntry.Created,
            LastModifiedAt = logbookEntry.LastModified
        };
    }
}