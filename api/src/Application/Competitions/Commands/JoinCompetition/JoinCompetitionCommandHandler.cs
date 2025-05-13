namespace Fishio.Application.Competitions.Commands.JoinCompetition;

public class JoinCompetitionCommandHandler : IRequestHandler<JoinCompetitionCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public JoinCompetitionCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(JoinCompetitionCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for joining a competition
        return Unit.Value;
    }
} 