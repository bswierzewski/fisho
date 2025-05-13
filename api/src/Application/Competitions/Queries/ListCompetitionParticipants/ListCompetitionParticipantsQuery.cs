namespace Fishio.Application.Competitions.Queries.ListCompetitionParticipants;

public record ListCompetitionParticipantsQuery : IRequest<List<CompetitionParticipantDto>>
{
    public int CompetitionId { get; init; }
    public string? Role { get; init; }
    public string? SearchTerm { get; init; }
}

public class ListCompetitionParticipantsQueryValidator : AbstractValidator<ListCompetitionParticipantsQuery>
{
    public ListCompetitionParticipantsQueryValidator()
    {
        RuleFor(x => x.CompetitionId)
            .NotEmpty().WithMessage("Competition ID is required");
    }
}

public record CompetitionParticipantDto
{
    public string UserId { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public bool IsGuest { get; init; }
    public DateTime JoinedAt { get; init; }
    public int CatchesCount { get; init; }
} 