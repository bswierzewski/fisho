using Fishio.Application.Common.Exceptions;

namespace Fishio.Application.Competitions.Commands.AssignJudge;

public class AssignJudgeCommandHandler : IRequestHandler<AssignJudgeCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AssignJudgeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(AssignJudgeCommand request, CancellationToken cancellationToken)
    {
        var competition = await _context.Competitions
            .Include(c => c.Participants) // Potrzebne do metody domenowej AssignJudge
            .FirstOrDefaultAsync(c => c.Id == request.CompetitionId, cancellationToken);

        if (competition == null)
        {
            throw new NotFoundException(nameof(Competition), request.CompetitionId.ToString());
        }

        var organizerId = _currentUserService.UserId;
        if (competition.OrganizerId != organizerId)
        {
            throw new ForbiddenAccessException("Tylko organizator może wyznaczać sędziów.");
        }

        var userToAssign = await _context.Users.FindAsync(new object[] { request.UserIdToAssignAsJudge }, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserIdToAssignAsJudge.ToString());

        // Używamy metody domenowej Competition.AssignJudge
        // Ta metoda powinna zawierać logikę sprawdzającą, czy można przypisać (np. status zawodów),
        // czy użytkownik już nie jest sędzią, czy nie jest gościem itp.
        try
        {
            var judgeParticipantEntry = competition.AssignJudge(userToAssign);
            await _context.SaveChangesAsync(cancellationToken);
            return judgeParticipantEntry.Id;
        }
        catch (InvalidOperationException ex)
        {
            throw new ApplicationException($"Nie można wyznaczyć sędziego: {ex.Message}", ex);
        }
    }
}
