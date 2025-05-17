using Fishio.Application.Common.Exceptions;

namespace Fishio.Application.Logbook.Commands.DeleteLogbookEntry;

public class DeleteLogbookEntryCommandHandler : IRequestHandler<DeleteLogbookEntryCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteLogbookEntryCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteLogbookEntryCommand request, CancellationToken cancellationToken)
    {
        var logbookEntry = await _context.LogbookEntries
                    .FirstOrDefaultAsync(le => le.Id == request.Id, cancellationToken);

        if (logbookEntry == null)
            throw new NotFoundException(nameof(LogbookEntry), request.Id.ToString());

        var currentUserId = _currentUserService.UserId;
        if (logbookEntry.UserId != currentUserId)
            throw new ForbiddenAccessException();

        _context.LogbookEntries.Remove(logbookEntry);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}