namespace Fishio.Application.Competitions.Commands.AddParticipant;

public class AddParticipantCommandHandler : IRequestHandler<AddParticipantCommand, int>
{
    private readonly IApplicationDbContext _context;

    public AddParticipantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(AddParticipantCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for adding a participant to a competition
        return 0;
    }
} 