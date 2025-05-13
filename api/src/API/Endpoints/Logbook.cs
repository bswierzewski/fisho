// Fishio.Api/Endpoints/LogbookEndpoints.cs
using Fishio.Application.Logbook.Commands.CreateLogbookEntry;
using Fishio.Application.Logbook.Commands.DeleteLogbookEntry;
using Fishio.Application.Logbook.Commands.UpdateLogbookEntry;
using Fishio.Application.Logbook.Queries.GetLogbookEntryDetails;
using Fishio.Application.Logbook.Queries.ListLogbookEntries;
// DTOs z warstwy Application
using Fishio.Application.Logbook.Queries; // np. LogbookEntryDto, LogbookEntryDetailsDto
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace Fishio.API.Endpoints;

public static class Logbook
{
    public static IEndpointRouteBuilder MapLogbookEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/logbook").WithTags("Logbook").RequireAuthorization(); // Wszystkie wymagają autoryzacji

        // POST /api/logbook
        // Tworzy nowy wpis w osobistym dzienniku połowów.
        group.MapPost("/", async (
            CreateLogbookEntryCommand command,
            ISender mediator,
            CancellationToken ct) =>
        {
            var entryId = await mediator.Send(command, ct);
            return TypedResults.CreatedAtRoute(
                "GetLogbookEntryDetails",
                new { entryId },
                new { id = entryId });
        })
        .WithName("CreateLogbookEntry")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        // GET /api/logbook
        // Pobiera listę wpisów z dziennika zalogowanego użytkownika.
        group.MapGet("/", async (ISender mediator, CancellationToken ct) =>
        {
            var query = new ListLogbookEntriesQuery(); // Zakłada pobranie UserId z Claims
            var result = await mediator.Send(query, ct);
            return TypedResults.Ok(result);
        })
        .WithName("ListMyLogbookEntries")
        .Produces<IEnumerable<LogbookEntryDto>>(StatusCodes.Status200OK);

        // GET /api/logbook/{entryId}
        // Pobiera szczegóły konkretnego wpisu z dziennika.
        group.MapGet("/{entryId:int}", async (
            int entryId,
            ISender mediator,
            CancellationToken ct) =>
        {
            var query = new GetLogbookEntryDetailsQuery { LogbookEntryId = entryId };
            var result = await mediator.Send(query, ct);
            return result != null ? TypedResults.Ok(result) : TypedResults.NotFound();
        })
        .WithName("GetLogbookEntryDetails")
        .Produces<LogbookEntryDetailsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // PUT /api/logbook/{entryId}
        // Aktualizuje istniejący wpis w dzienniku.
        group.MapPut("/{entryId:int}", async (
            int entryId,
            UpdateLogbookEntryCommand command,
            ISender mediator,
            CancellationToken ct) =>
        {
            if (entryId != command.LogbookEntryId)
            {
                return TypedResults.BadRequest("Mismatched Logbook Entry ID.");
            }
            await mediator.Send(command, ct);
            return TypedResults.NoContent();
        })
        .WithName("UpdateLogbookEntry")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status404NotFound);

        // DELETE /api/logbook/{entryId}
        // Usuwa wpis z dziennika.
        group.MapDelete("/{entryId:int}", async (
            int entryId,
            ISender mediator,
            CancellationToken ct) =>
        {
            var command = new DeleteLogbookEntryCommand { LogbookEntryId = entryId };
            await mediator.Send(command, ct);
            return TypedResults.NoContent();
        })
        .WithName("DeleteLogbookEntry")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
