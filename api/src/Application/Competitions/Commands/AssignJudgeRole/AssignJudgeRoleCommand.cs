namespace Fishio.Application.Competitions.Commands.AssignJudgeRole;

public record AssignJudgeRoleCommand : IRequest<Unit>
{
    public int CompetitionId { get; init; }
    public int ParticipantId { get; init; }
}

public class AssignJudgeRoleCommandValidator : AbstractValidator<AssignJudgeRoleCommand>
{
    public AssignJudgeRoleCommandValidator()
    {
        RuleFor(x => x.CompetitionId)
            .GreaterThan(0).WithMessage("Competition ID is required");

        RuleFor(x => x.ParticipantId)
            .GreaterThan(0).WithMessage("ParticipantId ID is required");
    }
}