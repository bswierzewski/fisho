namespace Fishio.Application.Competitions.Commands.AddParticipant;

public record AddParticipantCommand : IRequest<Unit>
{
    public Guid CompetitionId { get; init; }
    public string ParticipantId { get; init; } = string.Empty;
    public bool IsGuest { get; init; }
}

public class AddParticipantCommandValidator : AbstractValidator<AddParticipantCommand>
{
    public AddParticipantCommandValidator()
    {
        RuleFor(x => x.CompetitionId)
            .NotEmpty().WithMessage("Competition ID is required");

        RuleFor(x => x.ParticipantId)
            .NotEmpty().WithMessage("Participant ID is required");
    }
} 