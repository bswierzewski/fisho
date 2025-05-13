namespace Fishio.Application.Competitions.Commands.RevokeJudgeRole;

public class RevokeJudgeRoleCommandHandler : IRequestHandler<RevokeJudgeRoleCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public RevokeJudgeRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RevokeJudgeRoleCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for revoking judge role from a user
        return Unit.Value;
    }
} 