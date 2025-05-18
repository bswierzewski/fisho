using Fishio.Application.Common.Exceptions;

namespace Fishio.Application.Competitions.Commands.StartCompetition;

public class StartCompetitionCommandHandler : IRequestHandler<StartCompetitionCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    // private readonly TimeProvider _timeProvider; // Może być potrzebny, jeśli metoda domenowa go nie używa

    public StartCompetitionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(StartCompetitionCommand request, CancellationToken cancellationToken)
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
            throw new ForbiddenAccessException("Tylko organizator może rozpocząć zawody.");
        }

        try
        {
            competition.StartCompetition(); // Wywołanie metody domenowej
            // Metoda domenowa powinna sama sprawdzić warunki i rzucić InvalidOperationException
            // oraz ustawić TimeProvider, jeśli go potrzebuje do walidacji czasu.
            // Jeśli metoda domenowa nie używa TimeProvider, można go wstrzyknąć tutaj i przekazać.
        }
        catch (InvalidOperationException ex)
        {
            throw new ApplicationException($"Nie można rozpocząć zawodów: {ex.Message}", ex);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
