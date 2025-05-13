using Fishio.Application.Logbook.Commands.CreateLogbookEntry;
using Fishio.Application.Logbook.Commands.DeleteLogbookEntry;
using Fishio.Application.Logbook.Commands.UpdateLogbookEntry;
using Fishio.Application.Logbook.Queries.GetLogbookEntryDetails;
using Fishio.Application.Logbook.Queries.ListLogbookEntries;
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

        group.MapPost("/", CreateLogbookEntry)
            .WithName(nameof(CreateLogbookEntry));

        group.MapGet("/", ListMyLogbookEntries)
            .WithName(nameof(ListMyLogbookEntries));

        group.MapGet("/{entryId:int}", GetLogbookEntryDetailsById)
            .WithName(nameof(GetLogbookEntryDetailsById));

        group.MapPut("/{entryId:int}", UpdateLogbookEntryById)
            .WithName(nameof(UpdateLogbookEntryById));

        group.MapDelete("/{entryId:int}", DeleteLogbookEntryById)
            .WithName(nameof(DeleteLogbookEntryById));
    }

    private static async Task<IResult> CreateLogbookEntry(ISender sender, [FromBody] CreateLogbookEntryCommand command, CancellationToken ct)
    {
        var entryId = await sender.Send(command, ct);
        return TypedResults.CreatedAtRoute(nameof(GetLogbookEntryDetailsById), entryId.ToString(), new { id = entryId });
    }

    private static async Task<IResult> ListMyLogbookEntries(ISender sender, [AsParameters] ListLogbookEntriesQuery query, CancellationToken ct)
    {
        var entries = await sender.Send(query, ct);
        return TypedResults.Ok(entries);
    }

    private static async Task<IResult> GetLogbookEntryDetailsById(ISender sender, [FromRoute] int entryId, CancellationToken ct)
    {
         var query = new GetLogbookEntryDetailsQuery { LogbookEntryId = entryId };
         var entry = await sender.Send(query, ct);
         return entry != null ? TypedResults.Ok(entry) : TypedResults.NotFound();        
    }

    private static async Task<IResult> UpdateLogbookEntryById(ISender sender, [FromRoute] int entryId, [FromBody] UpdateLogbookEntryCommand command, CancellationToken ct)
    {
        if (command.LogbookEntryId == 0) return TypedResults.BadRequest("Mismatched LogbookEntryId.");
        else if (command.LogbookEntryId != entryId) return TypedResults.BadRequest("Mismatched Entry ID.");
        await sender.Send(command, ct);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteLogbookEntryById(ISender sender, [FromRoute] int entryId, CancellationToken ct)
    {
        var command = new DeleteLogbookEntryCommand { LogbookEntryId = entryId };
        await sender.Send(command, ct);
        return TypedResults.NoContent();
    }
}
