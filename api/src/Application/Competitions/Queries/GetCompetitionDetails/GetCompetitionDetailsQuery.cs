namespace Fishio.Application.Competitions.Queries.GetCompetitionDetails;

public record GetCompetitionDetailsQuery : IRequest<CompetitionDetailsDto>
{
    public Guid CompetitionId { get; init; }
}

public class GetCompetitionDetailsQueryValidator : AbstractValidator<GetCompetitionDetailsQuery>
{
    public GetCompetitionDetailsQueryValidator()
    {
        RuleFor(x => x.CompetitionId)
            .NotEmpty().WithMessage("Competition ID is required");
    }
}

public record CompetitionDetailsDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string OrganizerId { get; init; } = string.Empty;
    public string OrganizerName { get; init; } = string.Empty;
    public int ParticipantsCount { get; init; }
    public int JudgesCount { get; init; }
    public int CatchesCount { get; init; }
    public bool IsCurrentUserParticipant { get; init; }
    public bool IsCurrentUserJudge { get; init; }
    public bool IsCurrentUserOrganizer { get; init; }
    public List<CompetitionFishSpeciesDto> FishSpecies { get; init; } = new();
    public List<CompetitionParticipantDto> Participants { get; init; } = new();
    public List<CompetitionCatchDto> RecentCatches { get; init; } = new();
}

public record CompetitionFishSpeciesDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int CatchesCount { get; init; }
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

public record CompetitionCatchDto
{
    public Guid Id { get; init; }
    public string ParticipantId { get; init; } = string.Empty;
    public string ParticipantName { get; init; } = string.Empty;
    public Guid FishSpeciesId { get; init; }
    public string FishSpeciesName { get; init; } = string.Empty;
    public decimal Length { get; init; }
    public decimal? Weight { get; init; }
    public string? Notes { get; init; }
    public DateTime CaughtAt { get; init; }
    public string RecordedByUserId { get; init; } = string.Empty;
    public string RecordedByUserName { get; init; } = string.Empty;
}
