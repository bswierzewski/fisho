namespace Fishio.Application.Competitions.Commands.RecordFishCatch;

public class RecordFishCatchCommandHandler : IRequestHandler<RecordFishCatchCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public RecordFishCatchCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(RecordFishCatchCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for recording a fish catch in a competition
        return Unit.Value;
    }
} 