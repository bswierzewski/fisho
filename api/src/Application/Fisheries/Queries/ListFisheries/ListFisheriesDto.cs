namespace Fishio.Application.Fisheries.Queries.ListFisheries;

public record FisheryDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? Location { get; init; }
    public string? ImageUrl { get; set; }
    public int FishSpeciesCount { get; init; }
    public int TotalCatchesCount { get; init; }
    public DateTimeOffset? LastCatchDate { get; init; }
}