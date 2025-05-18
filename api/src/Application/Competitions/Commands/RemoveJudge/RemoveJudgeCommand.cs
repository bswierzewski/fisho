namespace Fishio.Application.Competitions.Commands.RemoveJudge;

public class RemoveJudgeCommand : IRequest<bool>
{
    public int CompetitionId { get; set; }
    public int JudgeParticipantEntryId { get; set; } // ID wpisu CompetitionParticipant dla sędziego
}

public class RemoveJudgeCommandValidator : AbstractValidator<RemoveJudgeCommand>
{
    public RemoveJudgeCommandValidator()
    {
        RuleFor(v => v.CompetitionId).NotEmpty();
        RuleFor(v => v.JudgeParticipantEntryId).NotEmpty();
    }
}
