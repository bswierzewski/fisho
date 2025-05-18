using Fishio.Application.Common.Exceptions;

namespace Fishio.Application.Competitions.Commands.AddParticipant;

public class AddParticipantCommandHandler : IRequestHandler<AddParticipantCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AddParticipantCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(AddParticipantCommand request, CancellationToken cancellationToken)
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
            throw new ForbiddenAccessException("Tylko organizator może dodawać uczestników.");
        }

        CompetitionParticipant participantEntry;

        if (request.UserId.HasValue)
        {
            var userToAdd = await _context.Users.FindAsync(new object[] { request.UserId.Value }, cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.UserId.Value.ToString());

            participantEntry = competition.AddParticipant(userToAdd, request.Role, true); // true = dodany przez organizatora
        }
        else // Dodawanie gościa
        {
            if (string.IsNullOrWhiteSpace(request.GuestName)) // Dodatkowa walidacja, choć powinna być w validatorze
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Nazwa gościa jest wymagana.");
            }
            participantEntry = competition.AddGuestParticipant(request.GuestName, request.Role, true);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return participantEntry.Id;
    }
}
