namespace Fishio.Application.Competitions.Commands.RevokeJudgeRole;

public record RevokeJudgeRoleCommand : IRequest<Unit>
{
    public int CompetitionId { get; init; }
    public int UserId { get; init; }
}

public class RevokeJudgeRoleCommandValidator : AbstractValidator<RevokeJudgeRoleCommand>
{
    public RevokeJudgeRoleCommandValidator()
    {
        RuleFor(x => x.CompetitionId)
            .NotEmpty().WithMessage("Competition ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
} 