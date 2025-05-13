namespace Fishio.Application.Competitions.Commands.AssignJudgeRole;

public class AssignJudgeRoleCommandHandler : IRequestHandler<AssignJudgeRoleCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public AssignJudgeRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AssignJudgeRoleCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for assigning judge role to a user
        return Unit.Value;
    }
} 