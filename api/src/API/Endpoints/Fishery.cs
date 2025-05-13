using Fishio.Application.Fisheries.Commands.CreateFishery;
using Fishio.Application.Fisheries.Commands.UpdateFishery;
using Fishio.Application.Fisheries.Queries.GetFisheryDetails;
using Fishio.Application.Fisheries.Queries.ListFisheries;

namespace Fishio.API.Endpoints;

public static class Fishery
{
    public static IEndpointRouteBuilder MapFisheryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/fisheries").WithTags("Fisheries");

        // POST /api/fisheries
        // Tworzy nowe łowisko. Wymaga: Zalogowany Użytkownik.
        group.MapPost("/", async (
            CreateFisheryCommand command,
            ISender mediator,
            CancellationToken ct) =>
        {
            var fisheryId = await mediator.Send(command, ct);
            return TypedResults.CreatedAtRoute(
                "GetFisheryDetails",
                new { fisheryId },
                new { id = fisheryId });
        })
        .WithName("CreateFishery")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequireAuthorization();

        // GET /api/fisheries
        // Pobiera listę wszystkich łowisk.
        group.MapGet("/", async (ISender mediator, CancellationToken ct) =>
        {
            var query = new ListFisheriesQuery();
            var result = await mediator.Send(query, ct);
            return TypedResults.Ok(result);
        })
        .WithName("ListFisheries")
        .Produces<IEnumerable<FisheryDto>>(StatusCodes.Status200OK);

        // GET /api/fisheries/{fisheryId}
        // Pobiera szczegóły konkretnego łowiska.
        group.MapGet("/{fisheryId:int}", async (
            int fisheryId,
            ISender mediator,
            CancellationToken ct) =>
        {
            var query = new GetFisheryDetailsQuery { FisheryId = fisheryId };
            var result = await mediator.Send(query, ct);
            return result != null ? TypedResults.Ok(result) : TypedResults.NotFound();
        })
        .WithName("GetFisheryDetails")
        .Produces<FisheryDetailsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // PUT /api/fisheries/{fisheryId}
        // Aktualizuje istniejące łowisko. Wymaga: Zalogowany Użytkownik (twórca lub admin).
        group.MapPut("/{fisheryId:int}", async (
            int fisheryId,
            UpdateFisheryCommand command,
            ISender mediator,
            CancellationToken ct) =>
        {
            if (fisheryId != command.FisheryId)
            {
                return TypedResults.BadRequest("Mismatched Fishery ID.");
            }
            await mediator.Send(command, ct);
            return TypedResults.NoContent();
        })
        .WithName("UpdateFishery")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("FisheryEditorPolicy"); // Wymaga polityki dla edytora łowiska

        return app;
    }
}
