using Fishio.Application.Common.Exceptions;

namespace Fishio.Application.Competitions.Commands.CancelCompetition;

public class CancelCompetitionCommandHandler : IRequestHandler<CancelCompetitionCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CancelCompetitionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(CancelCompetitionCommand request, CancellationToken cancellationToken)
    {
        var competition = await _context.Competitions
            .FirstOrDefaultAsync(c => c.Id == request.CompetitionId, cancellationToken);

        if (competition == null)
        {
            throw new NotFoundException(nameof(Competition), request.CompetitionId.ToString());
        }

        var organizerId = _currentUserService.UserId;
        if (competition.OrganizerId != organizerId)
        {
            throw new ForbiddenAccessException("Tylko organizator może anulować zawody.");
        }

        try
        {
            competition.CancelCompetition(request.Reason); // Wywołanie metody domenowej
            // Metoda domenowa powinna obsłużyć logikę zapisu powodu, jeśli encja ma takie pole
        }
        catch (InvalidOperationException ex)
        {
            throw new ApplicationException($"Nie można anulować zawodów: {ex.Message}", ex);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
