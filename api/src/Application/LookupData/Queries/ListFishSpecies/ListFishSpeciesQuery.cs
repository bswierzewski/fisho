namespace Fishio.Application.LookupData.Queries.ListFishSpecies;

public record ListFishSpeciesQuery : IRequest<List<FishSpeciesDto>>;

public record FishSpeciesDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal? MinLength { get; init; }
    public decimal? MaxLength { get; init; }
    public decimal? MinWeight { get; init; }
    public decimal? MaxWeight { get; init; }
    public bool IsProtected { get; init; }
} 