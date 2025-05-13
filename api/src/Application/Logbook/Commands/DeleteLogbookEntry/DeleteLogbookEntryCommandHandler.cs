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
        // TODO: Implement the logic for deleting a logbook entry
        return Unit.Value;
    }
} 