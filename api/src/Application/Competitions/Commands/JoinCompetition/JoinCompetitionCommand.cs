namespace Fishio.Application.Competitions.Commands.JoinCompetition;

public class JoinCompetitionCommand : IRequest<int> // Zwraca ID CompetitionParticipant
{
    public int CompetitionId { get; set; }
}

public class JoinCompetitionCommandValidator : AbstractValidator<JoinCompetitionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public JoinCompetitionCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;

        RuleFor(v => v.CompetitionId)
            .NotEmpty().WithMessage("ID zawodów jest wymagane.")
            .MustAsync(CompetitionMustExistAndBeOpenForJoining).WithMessage("Zawody nie istnieją, nie są otwarte lub już do nich dołączyłeś/aś.");
    }

    private async Task<bool> CompetitionMustExistAndBeOpenForJoining(int competitionId, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue) return false; // Musi być zalogowany

        var competition = await _context.Competitions
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == competitionId, cancellationToken);

        if (competition == null) return false;

        // Używamy metody domenowej lub logiki z niej
        bool canJoin = competition.Type == CompetitionType.Public &&
                       (competition.Status == CompetitionStatus.AcceptingRegistrations) && // Lub inne statusy pozwalające na dołączenie
                       !competition.Participants.Any(p => p.UserId == currentUserId.Value);

        return canJoin;
    }
}
