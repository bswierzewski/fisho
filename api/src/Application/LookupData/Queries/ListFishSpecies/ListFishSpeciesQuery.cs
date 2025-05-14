namespace Fishio.Application.LookupData.Queries.ListFishSpecies;

public record ListFishSpeciesQuery : IRequest<List<FishSpeciesDto>>;

public record FishSpeciesDto(int Id, string Name);