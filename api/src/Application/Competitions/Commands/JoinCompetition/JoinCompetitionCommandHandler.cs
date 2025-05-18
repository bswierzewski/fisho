namespace Fishio.Application.Competitions.Commands.JoinCompetition;

public class JoinCompetitionCommandHandler : IRequestHandler<JoinCompetitionCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public JoinCompetitionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(JoinCompetitionCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetOrProvisionDomainUserAsync(cancellationToken);
        if (currentUser == null || currentUser.Id == 0)
        {
            throw new UnauthorizedAccessException("Musisz być zalogowany, aby dołączyć do zawodów.");
        }

        var competition = await _context.Competitions
            .Include(c => c.Participants) // Potrzebne do wywołania metody domenowej AddParticipant
            .FirstOrDefaultAsync(c => c.Id == request.CompetitionId, cancellationToken);

        if (competition == null)
        {
            throw new NotFoundException(nameof(Competition), request.CompetitionId.ToString());
        }

        // Używamy metody domenowej Competition.AddParticipant
        // Ta metoda powinna zawierać logikę sprawdzającą, czy można dołączyć (status, typ, czy już nie jest uczestnikiem)
        try
        {
            var participantEntry = competition.AddParticipant(currentUser, ParticipantRole.Competitor, false); // false = nie dodany przez organizatora
            // EF Core powinien zacząć śledzić participantEntry, jeśli Competition jest śledzone
            // lub jeśli AddParticipant dodaje do kolekcji, która jest śledzona.
            // Jeśli nie, trzeba by dodać: _context.CompetitionParticipants.Add(participantEntry);

            await _context.SaveChangesAsync(cancellationToken);
            return participantEntry.Id;
        }
        catch (InvalidOperationException ex) // Przechwytywanie wyjątków z metody domenowej
        {
            // Można by tu logować lub mapować na bardziej specyficzny błąd HTTP
            throw new ApplicationException($"Nie można dołączyć do zawodów: {ex.Message}", ex);
        }
    }
}
