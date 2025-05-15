using Fishio.Application.LookupData.Queries.ListCategoryDefinitions;
using Fishio.Application.LookupData.Queries.ListEnumValues;
using Fishio.Application.LookupData.Queries.ListFishSpecies;

namespace Fishio.API.Endpoints;

public static class LookupDataEndpoints
{
    public static void MapLookupDataEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/lookup")
            .WithTags("LookupData")
            .WithOpenApi();

        // --- Dedykowane Endpointy dla Enumów ---
        group.MapGet("/enums/category-types", async (ISender sender) =>
            {
                var query = new ListEnumValuesQuery(nameof(CategoryType));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetCategoryTypeEnumValues")
            .WithSummary("Pobiera wartości dla enuma CategoryType")
            .Produces<IEnumerable<EnumValueDto>>();

        group.MapGet("/enums/category-metrics", async (ISender sender) =>
            {
                var query = new ListEnumValuesQuery(nameof(CategoryMetric));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetCategoryMetricEnumValues")
            .WithSummary("Pobiera wartości dla enuma CategoryMetric")
            .Produces<IEnumerable<EnumValueDto>>();

        group.MapGet("/enums/category-calculation-logics", async (ISender sender) =>
            {
                var query = new ListEnumValuesQuery(nameof(CategoryCalculationLogic));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetCategoryCalculationLogicEnumValues")
            .WithSummary("Pobiera wartości dla enuma CategoryCalculationLogic")
            .Produces<IEnumerable<EnumValueDto>>();

        group.MapGet("/enums/category-entity-types", async (ISender sender) =>
            {
                var query = new ListEnumValuesQuery(nameof(CategoryEntityType));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetCategoryEntityTypeEnumValues")
            .WithSummary("Pobiera wartości dla enuma CategoryEntityType")
            .Produces<IEnumerable<EnumValueDto>>();

        group.MapGet("/enums/competition-statuses", async (ISender sender) =>
            {
                var query = new ListEnumValuesQuery(nameof(CompetitionStatus));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetCompetitionStatusEnumValues")
            .WithSummary("Pobiera wartości dla enuma CompetitionStatus")
            .Produces<IEnumerable<EnumValueDto>>();

        group.MapGet("/enums/competition-type", async (ISender sender) =>
            {
                var query = new ListEnumValuesQuery(nameof(CompetitionType));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetCompetitionTypeEnumValues")
            .WithSummary("Pobiera wartości dla enuma CompetitionType")
            .Produces<IEnumerable<EnumValueDto>>();

        group.MapGet("/enums/participant-role", async (ISender sender) =>
            {
                var query = new ListEnumValuesQuery(nameof(ParticipantRole));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetParticipantRoleEnumValues")
            .WithSummary("Pobiera wartości dla enuma ParticipantRole")
            .Produces<IEnumerable<EnumValueDto>>();

        // Endpoint do pobierania definicji kategorii
        group.MapGet("/category-definitions", async (
                [AsParameters] ListCategoryDefinitionsQuery query,
                ISender sender) =>
            {
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetCategoryDefinitions")
            .Produces<IEnumerable<CategoryDefinitionDto>>();

        group.MapGet("/fishspecies", GetAllFishSpecies)
            .WithName(nameof(GetAllFishSpecies));
    }

    private static async Task<IResult> GetAllFishSpecies(ISender sender, [AsParameters] ListFishSpeciesQuery query, CancellationToken ct)
    {
        var species = await sender.Send(query, ct);
        return TypedResults.Ok(species);
    }
}
