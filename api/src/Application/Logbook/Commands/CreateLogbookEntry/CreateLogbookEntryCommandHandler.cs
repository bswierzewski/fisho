namespace Fishio.Application.Logbook.Commands.CreateLogbookEntry;

public class CreateLogbookEntryCommandHandler : IRequestHandler<CreateLogbookEntryCommand, Unit>
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

    public async Task<Unit> Handle(CreateLogbookEntryCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for creating a logbook entry
        return Unit.Value;
    }
} 