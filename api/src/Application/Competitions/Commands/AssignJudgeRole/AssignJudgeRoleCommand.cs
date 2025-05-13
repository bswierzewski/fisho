namespace Fishio.Application.Competitions.Commands.AssignJudgeRole;

public record AssignJudgeRoleCommand : IRequest<Unit>
{
    public Guid CompetitionId { get; init; }
    public string UserId { get; init; } = string.Empty;
}

public class AssignJudgeRoleCommandValidator : AbstractValidator<AssignJudgeRoleCommand>
{
    public AssignJudgeRoleCommandValidator()
    {
        RuleFor(x => x.CompetitionId)
            .NotEmpty().WithMessage("Competition ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
} 