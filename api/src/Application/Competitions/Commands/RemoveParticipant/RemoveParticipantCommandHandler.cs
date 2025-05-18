using Fishio.Application.Common.Exceptions;

namespace Fishio.Application.Competitions.Commands.RemoveParticipant;

public class RemoveParticipantCommandHandler : IRequestHandler<RemoveParticipantCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public RemoveParticipantCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(RemoveParticipantCommand request, CancellationToken cancellationToken)
    {
        var competition = await _context.Competitions
            .Include(c => c.Participants) // Potrzebne do metody domenowej
            .FirstOrDefaultAsync(c => c.Id == request.CompetitionId, cancellationToken);

        if (competition == null)
        {
            throw new NotFoundException(nameof(Competition), request.CompetitionId.ToString());
        }

        var organizerId = _currentUserService.UserId;
        if (competition.OrganizerId != organizerId)
        {
            throw new ForbiddenAccessException("Tylko organizator może usuwać uczestników.");
        }

        var participantToRemove = competition.Participants.FirstOrDefault(p => p.Id == request.ParticipantEntryId);
        if (participantToRemove == null)
        {
            throw new NotFoundException("Uczestnik o podanym ID nie został znaleziony w tych zawodach.", request.ParticipantEntryId.ToString());
        }

        competition.RemoveParticipant(participantToRemove); // Używamy metody domenowej

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
