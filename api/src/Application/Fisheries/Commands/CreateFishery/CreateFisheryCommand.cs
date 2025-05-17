namespace Fishio.Application.Fisheries.Commands.CreateFishery;

public record CreateFisheryCommand : IRequest<int>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public List<int> FishSpeciesIds { get; init; } = new();
}