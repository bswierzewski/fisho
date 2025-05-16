namespace Fishio.Application.Logbook.Commands.CreateLogbookEntry;

public class CreateLogbookEntryCommandHandler : IRequestHandler<CreateLogbookEntryCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateLogbookEntryCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateLogbookEntryCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.DomainUserId;
        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException("Użytkownik musi być zalogowany, aby dodać wpis do dziennika.");
        }

        var logbookEntry = new LogbookEntry(
            userId.Value,
            request.PhotoUrl,
            request.CaughtAt,
            request.Length,
            request.Weight,
            request.Notes,
            request.FishSpeciesId,
            request.FisheryId
        );
        // Pola audytu (Created, CreatedBy etc.) zostaną ustawione przez BaseAuditableEntity
        // lub można je ustawić tutaj, jeśli ICurrentUserService dostarcza CreatedBy

        _context.LogbookEntries.Add(logbookEntry);
        // await _logbookRepository.AddAsync(logbookEntry, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return logbookEntry.Id;
    }
} 