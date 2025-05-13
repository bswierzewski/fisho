namespace Fishio.Application.Competitions.Commands.RemoveParticipant;

public class RemoveParticipantCommandHandler : IRequestHandler<RemoveParticipantCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public RemoveParticipantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RemoveParticipantCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for removing a participant from a competition
        return Unit.Value;
    }
} 