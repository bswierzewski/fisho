namespace Fishio.Application.Fisheries.Commands.UpdateFishery;

public record UpdateFisheryCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string? ImageUrl { get; init; }
    public List<int> FishSpeciesIds { get; init; } = new();
}