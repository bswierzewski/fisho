using Fishio.Application.Common.Models;
using Fishio.Application.Logbook.Queries.ListLogbookEntries;
using Fishio.Application.LogbookEntries.Commands.CreateLogbookEntry;
using Fishio.Application.LogbookEntries.Commands.DeleteLogbookEntry;
using Fishio.Application.LogbookEntries.Commands.UpdateLogbookEntry;
using Fishio.Application.LogbookEntries.Queries.GetLogbookEntryById;
using Microsoft.AspNetCore.Mvc;

namespace Fishio.API.Endpoints;

public static class LogbookEndpoints
{
    public static void MapLogbookEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/logbook")
            .WithTags("Logbook")
            .WithOpenApi()
            .RequireAuthorization();

        group.MapPost("/", CreateNewLogbookEntry)
            .WithName(nameof(CreateNewLogbookEntry))
            .Produces<object>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet("/", GetCurrentUserLogbookEntries)
            .WithName(nameof(GetCurrentUserLogbookEntries))
            .Produces<PaginatedList<Application.Logbook.Queries.ListLogbookEntries.LogbookEntryDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        group.MapGet("/{id:int}", GetLogbookEntryDetailsById) // Zmieniono nazwę metody
            .WithName(nameof(GetLogbookEntryDetailsById))
            .Produces<Application.LogbookEntries.Queries.GetLogbookEntryById.LogbookEntryDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound) // Jeśli handler zwróci null
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden); // Jeśli handler rzuci Forbidden

        group.MapPut("/{id:int}", UpdateExistingLogbookEntry) // Zmieniono nazwę metody
            .WithName(nameof(UpdateExistingLogbookEntry))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        group.MapDelete("/{id:int}", DeleteExistingLogbookEntry) // Zmieniono nazwę metody
            .WithName(nameof(DeleteExistingLogbookEntry))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);
    }

    private static async Task<IResult> CreateNewLogbookEntry(
        ISender sender,
        [FromForm] CreateLogbookEntryCommand command,
        CancellationToken ct)
    {
        var entryId = await sender.Send(command, ct);
        return TypedResults.Created($"/api/logbook/{entryId}", new { Id = entryId });
    }

    private static async Task<IResult> GetCurrentUserLogbookEntries(
        ISender sender,
        [AsParameters] GetUserLogbookEntriesQuery query,
        CancellationToken ct)
    {
        var entries = await sender.Send(query, ct);
        return TypedResults.Ok(entries);
    }

    private static async Task<IResult> GetLogbookEntryDetailsById( // Zmieniono nazwę metody
        ISender sender,
        int id,
        CancellationToken ct)
    {
        var query = new GetLogbookEntryByIdQuery(id);
        var entry = await sender.Send(query, ct);
        return entry != null ? TypedResults.Ok(entry) : TypedResults.NotFound();
    }

    private static async Task<IResult> UpdateExistingLogbookEntry( // Zmieniono nazwę metody
        ISender sender,
        int id,
        [FromForm] UpdateLogbookEntryCommand command,
        CancellationToken ct)
    {
        if (id != command.Id)
        {
            return TypedResults.BadRequest("ID w ścieżce nie zgadza się z ID w ciele żądania.");
        }
        await sender.Send(command, ct); // Handler rzuca wyjątki w przypadku błędu
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteExistingLogbookEntry( // Zmieniono nazwę metody
        ISender sender,
        int id,
        CancellationToken ct)
    {
        var command = new DeleteLogbookEntryCommand(id);
        await sender.Send(command, ct); // Handler rzuca wyjątki w przypadku błędu
        return TypedResults.NoContent();
    }
}
