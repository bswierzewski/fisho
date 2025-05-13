namespace Fishio.Application.Competitions.Commands.RemoveParticipant;

public record RemoveParticipantCommand : IRequest<Unit>
{
    public int CompetitionId { get; init; }
    public int ParticipantId { get; init; }
}

public class RemoveParticipantCommandValidator : AbstractValidator<RemoveParticipantCommand>
{
    public RemoveParticipantCommandValidator()
    {
        RuleFor(x => x.CompetitionId)
            .GreaterThan(0).WithMessage("Competition ID is required");

        RuleFor(x => x.ParticipantId)
            .GreaterThan(0).WithMessage("Participant ID is required");
    }
} 