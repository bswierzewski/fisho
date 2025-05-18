using Fishio.Application.LookupData.Queries.GetCategoryDefinitions;
using Fishio.Application.LookupData.Queries.GetFishSpeciesQuery;
using Fishio.Application.LookupData.Queries.ListEnumValues;

namespace Fishio.API.Endpoints;

public static class LookupDataEndpoints
{
    public static void MapLookupDataEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/lookup")
            .WithTags("LookupData")
            .WithOpenApi();
        // Te endpointy są publiczne, więc nie dodajemy .RequireAuthorization() do grupy

        group.MapGet("/fish-species", GetAllFishSpecies)
            .WithName(nameof(GetAllFishSpecies))
            .Produces<IEnumerable<FishSpeciesDto>>(StatusCodes.Status200OK);

        group.MapGet("/category-definitions", GetGlobalCategoryDefinitions)
            .WithName(nameof(GetGlobalCategoryDefinitions))
            .Produces<IEnumerable<CategoryDefinitionDto>>(StatusCodes.Status200OK);

        // --- Dedykowane Endpointy dla Enumów ---
        group.MapGet("/enums/category-types", async (ISender sender) =>
            {
                var query = new GetListEnumValuesQuery(nameof(CategoryType));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetCategoryTypeEnumValues")
            .WithSummary("Pobiera wartości dla enuma CategoryType")
            .Produces<IEnumerable<EnumValueDto>>();

        group.MapGet("/enums/category-metrics", async (ISender sender) =>
            {
                var query = new GetListEnumValuesQuery(nameof(CategoryMetric));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetCategoryMetricEnumValues")
            .WithSummary("Pobiera wartości dla enuma CategoryMetric")
            .Produces<IEnumerable<EnumValueDto>>();

        group.MapGet("/enums/category-calculation-logics", async (ISender sender) =>
            {
                var query = new GetListEnumValuesQuery(nameof(CategoryCalculationLogic));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetCategoryCalculationLogicEnumValues")
            .WithSummary("Pobiera wartości dla enuma CategoryCalculationLogic")
            .Produces<IEnumerable<EnumValueDto>>();

        group.MapGet("/enums/category-entity-types", async (ISender sender) =>
            {
                var query = new GetListEnumValuesQuery(nameof(CategoryEntityType));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetCategoryEntityTypeEnumValues")
            .WithSummary("Pobiera wartości dla enuma CategoryEntityType")
            .Produces<IEnumerable<EnumValueDto>>();

        group.MapGet("/enums/competition-statuses", async (ISender sender) =>
            {
                var query = new GetListEnumValuesQuery(nameof(CompetitionStatus));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetCompetitionStatusEnumValues")
            .WithSummary("Pobiera wartości dla enuma CompetitionStatus")
            .Produces<IEnumerable<EnumValueDto>>();

        group.MapGet("/enums/competition-type", async (ISender sender) =>
            {
                var query = new GetListEnumValuesQuery(nameof(CompetitionType));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetCompetitionTypeEnumValues")
            .WithSummary("Pobiera wartości dla enuma CompetitionType")
            .Produces<IEnumerable<EnumValueDto>>();

        group.MapGet("/enums/participant-role", async (ISender sender) =>
            {
                var query = new GetListEnumValuesQuery(nameof(ParticipantRole));
                var result = await sender.Send(query);
                return TypedResults.Ok(result);
            })
            .WithName("GetParticipantRoleEnumValues")
            .WithSummary("Pobiera wartości dla enuma ParticipantRole")
            .Produces<IEnumerable<EnumValueDto>>();
    }

    private static async Task<IResult> GetAllFishSpecies(
            ISender sender,
            [AsParameters] GetFishSpeciesQuery query, // Nawet jeśli query jest puste, [AsParameters] jest OK
            CancellationToken ct)
    {
        var result = await sender.Send(query, ct);
        return TypedResults.Ok(result);
    }

    private static async Task<IResult> GetGlobalCategoryDefinitions(
        ISender sender,
        [AsParameters] GetCategoryDefinitionsQuery query, // Podobnie tutaj
        CancellationToken ct)
    {
        var result = await sender.Send(query, ct);
        return TypedResults.Ok(result);
    }
}
