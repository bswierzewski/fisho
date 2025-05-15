using Fishio.Application.LookupData.Queries.ListFishSpecies;

namespace Fishio.API.Endpoints;

public static class LookupDataEndpoints
{
    public static void MapLookupDataEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/lookup")
            .WithTags("LookupData")
            .WithOpenApi();

        group.MapGet("/fishspecies", GetAllFishSpecies)
            .WithName(nameof(GetAllFishSpecies));
    }

    private static async Task<IResult> GetAllFishSpecies(ISender sender, [AsParameters] ListFishSpeciesQuery query, CancellationToken ct)
    {
        var species = await sender.Send(query, ct);
        return TypedResults.Ok(species);
    }
}
