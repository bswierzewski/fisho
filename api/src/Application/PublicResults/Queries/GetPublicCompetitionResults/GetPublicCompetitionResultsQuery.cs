namespace Fishio.Application.PublicResults.Queries.GetPublicCompetitionResults;

public record GetPublicCompetitionResultsQuery : IRequest<PublicCompetitionResultsDto>
{
    public string Token { get; init; } = string.Empty;
}

public class GetPublicCompetitionResultsQueryValidator : AbstractValidator<GetPublicCompetitionResultsQuery>
{
    public GetPublicCompetitionResultsQueryValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required");
    }
}

public record PublicCompetitionResultsDto
{
    public string CompetitionName { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public List<PublicParticipantResultDto> Results { get; init; } = new();
    public List<PublicCatchDto> RecentCatches { get; init; } = new();
}

public record PublicParticipantResultDto
{
    public string ParticipantName { get; init; } = string.Empty;
    public int Position { get; init; }
    public int TotalPoints { get; init; }
    public int CatchesCount { get; init; }
    public decimal? BiggestFishLength { get; init; }
    public decimal? BiggestFishWeight { get; init; }
}

public record PublicCatchDto
{
    public string ParticipantName { get; init; } = string.Empty;
    public string FishSpeciesName { get; init; } = string.Empty;
    public decimal Length { get; init; }
    public decimal? Weight { get; init; }
    public DateTime CaughtAt { get; init; }
} 