namespace Fishio.Application.Competitions.Commands.RevokeJudgeRole;

public record RevokeJudgeRoleCommand : IRequest<Unit>
{
    public Guid CompetitionId { get; init; }
    public string UserId { get; init; } = string.Empty;
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