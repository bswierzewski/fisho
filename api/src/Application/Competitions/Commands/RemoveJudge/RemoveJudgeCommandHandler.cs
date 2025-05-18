using Fishio.Application.Common.Exceptions;

namespace Fishio.Application.Competitions.Commands.RemoveJudge;

public class RemoveJudgeCommandHandler : IRequestHandler<RemoveJudgeCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public RemoveJudgeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(RemoveJudgeCommand request, CancellationToken cancellationToken)
    {
        var competition = await _context.Competitions
            .Include(c => c.Participants) // Potrzebne do metody domenowej RemoveJudge
            .FirstOrDefaultAsync(c => c.Id == request.CompetitionId, cancellationToken);

        if (competition == null)
        {
            throw new NotFoundException(nameof(Competition), request.CompetitionId.ToString());
        }

        var organizerId = _currentUserService.UserId;
        if (competition.OrganizerId != organizerId)
        {
            throw new ForbiddenAccessException("Tylko organizator może usuwać sędziów.");
        }

        var judgeParticipantToRemove = competition.Participants
            .FirstOrDefault(p => p.Id == request.JudgeParticipantEntryId && p.Role == Fishio.Domain.Enums.ParticipantRole.Judge);

        if (judgeParticipantToRemove == null)
        {
            throw new NotFoundException("Sędzia o podanym ID nie został znaleziony w tych zawodach.", request.JudgeParticipantEntryId.ToString());
        }

        // Używamy metody domenowej Competition.RemoveJudge
        // Ta metoda powinna zawierać logikę sprawdzającą, czy można usunąć (np. status zawodów)
        try
        {
            competition.RemoveJudge(judgeParticipantToRemove);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (InvalidOperationException ex)
        {
            throw new ApplicationException($"Nie można usunąć sędziego: {ex.Message}", ex);
        }
    }
}
