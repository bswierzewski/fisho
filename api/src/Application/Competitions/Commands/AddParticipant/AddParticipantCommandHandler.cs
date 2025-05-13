namespace Fishio.Application.Competitions.Commands.AddParticipant;

public class AddParticipantCommandHandler : IRequestHandler<AddParticipantCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public AddParticipantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddParticipantCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for adding a participant to a competition
        return Unit.Value;
    }
} 