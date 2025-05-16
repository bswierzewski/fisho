using Fishio.Application.Fisheries.Commands.CreateFishery;
using Fishio.Application.Fisheries.Commands.UpdateFishery;
using Fishio.Application.Fisheries.Queries.GetFisheryDetails;
using Fishio.Application.Fisheries.Queries.ListFisheries;
using Microsoft.AspNetCore.Mvc;

namespace Fishio.API.Endpoints;

public static class FisheriesEndpoints
{
    public static void MapFisheriesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/fisheries")
            .WithTags("Fisheries")
            .WithOpenApi();

        group.MapPost("/", CreateFishery)
            .WithName(nameof(CreateFishery))
            .RequireAuthorization();

        group.MapGet("/", ListAllFisheries)
            .WithName(nameof(ListAllFisheries));

        group.MapGet("/{fisheryId:int}", GetFisheryDetailsById)
            .WithName(nameof(GetFisheryDetailsById));

        group.MapPut("/{fisheryId:int}", UpdateFisheryById)
            .WithName(nameof(UpdateFisheryById))
            .RequireAuthorization("FisheryEditorPolicy");
    }

    private static async Task<IResult> CreateFishery(ISender sender, [FromBody] CreateFisheryCommand command, CancellationToken ct)
    {
        var fisheryId = await sender.Send(command, ct);
        return TypedResults.CreatedAtRoute(nameof(GetFisheryDetailsById), fisheryId.ToString(), new { id = fisheryId });
    }

    private static async Task<IResult> ListAllFisheries(ISender sender, [AsParameters] ListFisheriesQuery query, CancellationToken ct)
    {
        var fisheries = await sender.Send(query, ct);
        return TypedResults.Ok(fisheries);
    }

    private static async Task<IResult> GetFisheryDetailsById(ISender sender, [FromRoute] int fisheryId, CancellationToken ct)
    {
        var query = new GetFisheryDetailsQuery { Id = fisheryId };
        var fishery = await sender.Send(query, ct);
        return fishery != null ? TypedResults.Ok(fishery) : TypedResults.NotFound();
    }

    private static async Task<IResult> UpdateFisheryById(ISender sender, [FromRoute] int fisheryId, [FromBody] UpdateFisheryCommand command, CancellationToken ct)
    {
        var updateCommand = command;
        if (updateCommand.Id == 0) return TypedResults.BadRequest("Empty FisheryId.");
        else if (updateCommand.Id != fisheryId) return TypedResults.BadRequest("Mismatched ID.");
        await sender.Send(updateCommand, ct);
        return TypedResults.Ok();
    }
}
