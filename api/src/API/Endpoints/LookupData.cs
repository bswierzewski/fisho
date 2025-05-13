using Fishio.Application.LookupData.Queries.ListFishSpecies;
using Fishio.Application.LookupData.Queries.ListScoringCategories;

namespace Fishio.API.Endpoints;

public static class LookupData
{
    public static IEndpointRouteBuilder MapLookupDataEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api").WithTags("LookupData");

        // GET /api/fishspecies
        // Pobiera listę predefiniowanych gatunków ryb.
        group.MapGet("/fishspecies", async (ISender mediator, CancellationToken ct) =>
        {
            var query = new ListFishSpeciesQuery();
            var result = await mediator.Send(query, ct);
            return TypedResults.Ok(result);
        })
        .WithName("ListFishSpecies")
        .Produces<IEnumerable<FishSpeciesDto>>(StatusCodes.Status200OK);

        // GET /api/scoringcategories
        // Pobiera listę dostępnych głównych kategorii punktacji.
        group.MapGet("/scoringcategories", async (ISender mediator, CancellationToken ct) =>
        {
            var query = new ListScoringCategoriesQuery();
            var result = await mediator.Send(query, ct);
            return TypedResults.Ok(result);
        })
        .WithName("ListScoringCategoryOptions")
        .Produces<IEnumerable<ScoringCategoryDto>>(StatusCodes.Status200OK);

        return app;
    }
}
