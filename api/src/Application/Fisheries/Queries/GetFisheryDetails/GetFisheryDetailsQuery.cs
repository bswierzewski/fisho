namespace Fishio.Application.Fisheries.Queries.GetFisheryDetails;

public record GetFisheryDetailsQuery : IRequest<FisheryDetailsDto>
{
    public int FisheryId { get; init; }
}

public class GetFisheryDetailsQueryValidator : AbstractValidator<GetFisheryDetailsQuery>
{
    public GetFisheryDetailsQueryValidator()
    {
        RuleFor(x => x.FisheryId)
            .NotEmpty().WithMessage("Fishery ID is required");
    }
}

public record FisheryDetailsDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string CreatedByUserId { get; init; } = string.Empty;
    public string CreatedByUserName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? LastModifiedAt { get; init; }
    public List<FisheryFishSpeciesDto> FishSpecies { get; init; } = new();
    public List<FisheryStatisticsDto> Statistics { get; init; } = new();
}

public record FisheryFishSpeciesDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int CatchesCount { get; init; }
    public decimal? AverageLength { get; init; }
    public decimal? AverageWeight { get; init; }
}

public record FisheryStatisticsDto
{
    public int TotalCatchesCount { get; init; }
    public int TotalAnglers { get; init; }
    public int TotalCompetitions { get; init; }
    public DateTime? LastCatchDate { get; init; }
} 