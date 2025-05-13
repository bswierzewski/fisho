namespace Fishio.Application.Competitions.Commands.RemoveParticipant;

public record RemoveParticipantCommand : IRequest<Unit>
{
    public Guid CompetitionId { get; init; }
    public string ParticipantId { get; init; } = string.Empty;
}

public class RemoveParticipantCommandValidator : AbstractValidator<RemoveParticipantCommand>
{
    public RemoveParticipantCommandValidator()
    {
        RuleFor(x => x.CompetitionId)
            .NotEmpty().WithMessage("Competition ID is required");

        RuleFor(x => x.ParticipantId)
            .NotEmpty().WithMessage("Participant ID is required");
    }
} 