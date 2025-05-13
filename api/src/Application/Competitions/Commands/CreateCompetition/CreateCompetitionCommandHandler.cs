namespace Fishio.Application.Competitions.Commands.CreateCompetition;

public class CreateCompetitionCommandHandler : IRequestHandler<CreateCompetitionCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateCompetitionCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateCompetitionCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for creating a competition
        return 0;
    }
}
