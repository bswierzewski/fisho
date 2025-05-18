namespace Fishio.Application.Competitions.Commands.RemoveParticipant;

public class RemoveParticipantCommand : IRequest<bool>
{
    public int CompetitionId { get; set; }
    public int ParticipantEntryId { get; set; } // ID wpisu CompetitionParticipant
}

public class RemoveParticipantCommandValidator : AbstractValidator<RemoveParticipantCommand>
{
    public RemoveParticipantCommandValidator()
    {
        RuleFor(v => v.CompetitionId).NotEmpty();
        RuleFor(v => v.ParticipantEntryId).NotEmpty();
    }
}
