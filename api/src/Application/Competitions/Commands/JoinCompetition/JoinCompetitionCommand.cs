namespace Fishio.Application.Competitions.Commands.JoinCompetition;

public record JoinCompetitionCommand : IRequest<Unit>
{
    public Guid CompetitionId { get; init; }
}

public class JoinCompetitionCommandValidator : AbstractValidator<JoinCompetitionCommand>
{
    public JoinCompetitionCommandValidator()
    {
        RuleFor(x => x.CompetitionId)
            .NotEmpty().WithMessage("Competition ID is required");
    }
} 