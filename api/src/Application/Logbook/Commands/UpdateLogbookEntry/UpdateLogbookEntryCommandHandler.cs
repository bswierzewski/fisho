namespace Fishio.Application.Logbook.Commands.UpdateLogbookEntry;

public class UpdateLogbookEntryCommandHandler : IRequestHandler<UpdateLogbookEntryCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateLogbookEntryCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateLogbookEntryCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for updating a logbook entry
        return Unit.Value;
    }
} 