// Fishio.Api/Endpoints/CompetitionEndpoints.cs
using Fishio.Application.Competitions.Commands.CreateCompetition;
using Fishio.Application.Competitions.Commands.AddParticipant;
using Fishio.Application.Competitions.Commands.AssignJudgeRole;
using Fishio.Application.Competitions.Commands.JoinCompetition;
using Fishio.Application.Competitions.Commands.RecordFishCatch;
using Fishio.Application.Competitions.Commands.RemoveParticipant;
using Fishio.Application.Competitions.Commands.RevokeJudgeRole;
// using Fishio.Application.Competitions.Commands.UpdateCompetition; // Jeśli dodasz edycję
using Fishio.Application.Competitions.Queries.GetCompetitionDetails;
using Fishio.Application.Competitions.Queries.ListCompetitions;
using Fishio.Application.Competitions.Queries.ListCompetitionParticipants;
using Fishio.Application.Competitions.Queries.ListMyCompetitions;
using Fishio.Application.Competitions.Queries.ListCompetitionCatches;
// DTOs z warstwy Application
using Fishio.Application.Competitions.Queries; // np. CompetitionDto, CompetitionDetailsDto, ParticipantDto, FishCatchDto
using Fishio.Domain.Enums; // Dla CompetitionType
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace Fishio.API.Endpoints;

public static class Competition
{
    public static IEndpointRouteBuilder MapCompetitionEndpoints(this IEndpointRouteBuilder app)
    {
        var competitionsGroup = app.MapGroup("/api/competitions").WithTags("Competitions");

        // POST /api/competitions
        // Tworzy nowe zawody. Wymaga: Zalogowany Użytkownik.
        competitionsGroup.MapPost("/", async (
            CreateCompetitionCommand command,
            ISender mediator,
            CancellationToken ct) =>
        {
            var competitionId = await mediator.Send(command, ct);
            return TypedResults.CreatedAtRoute(
                "GetCompetitionDetails",
                new { competitionId },
                new { id = competitionId });
        })
        .WithName("CreateCompetition")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequireAuthorization();

        // GET /api/competitions?type=Open
        // Pobiera listę otwartych zawodów lub wszystkich (jeśli type nie jest podany).
        competitionsGroup.MapGet("/", async (
            [FromQuery] CompetitionType? type, // Z Domain.Enums
            ISender mediator,
            CancellationToken ct) =>
        {
            var query = new ListCompetitionsQuery { TypeFilter = type };
            var result = await mediator.Send(query, ct);
            return TypedResults.Ok(result);
        })
        .WithName("ListOpenCompetitions")
        .Produces<IEnumerable<CompetitionDto>>(StatusCodes.Status200OK);

        // GET /api/competitions/mine
        // Pobiera listę "Moje Zawody". Wymaga: Zalogowany Użytkownik.
        competitionsGroup.MapGet("/mine", async (ISender mediator, CancellationToken ct) =>
        {
            var query = new ListMyCompetitionsQuery(); // Zakłada pobranie UserId z Claims
            var result = await mediator.Send(query, ct);
            return TypedResults.Ok(result);
        })
        .WithName("ListMyCompetitions")
        .Produces<IEnumerable<CompetitionDto>>(StatusCodes.Status200OK)
        .RequireAuthorization();

        // GET /api/competitions/{competitionId}
        // Pobiera szczegóły konkretnych zawodów.
        competitionsGroup.MapGet("/{competitionId:int}", async (
            int competitionId,
            ISender mediator,
            CancellationToken ct) =>
        {
            var query = new GetCompetitionDetailsQuery { CompetitionId = competitionId };
            var result = await mediator.Send(query, ct);
            return result != null ? TypedResults.Ok(result) : TypedResults.NotFound();
        })
        .WithName("GetCompetitionDetails")
        .Produces<CompetitionDetailsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // --- Participants Sub-Group ---
        var participantsGroup = competitionsGroup.MapGroup("/{competitionId:int}/participants");

        // POST /api/competitions/{competitionId}/participants/join
        // Pozwala zalogowanemu użytkownikowi dołączyć do otwartych zawodów. Wymaga: Zalogowany Użytkownik.
        participantsGroup.MapPost("/join", async (
            int competitionId,
            ISender mediator,
            CancellationToken ct) =>
        {
            var command = new JoinCompetitionCommand { CompetitionId = competitionId };
            await mediator.Send(command, ct);
            return TypedResults.Ok();
        })
        .WithName("JoinOpenCompetition")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest) // Np. już dołączył, zawody nie są otwarte
        .RequireAuthorization();

        // POST /api/competitions/{competitionId}/participants
        // Organizator dodaje uczestnika. Wymaga: Organizator.
        participantsGroup.MapPost("/", async (
            int competitionId,
            AddParticipantCommand command, // Command zawiera UserId lub GuestName/GuestIdentifier, Role
            ISender mediator,
            CancellationToken ct) =>
        {
            command.CompetitionId = competitionId; // Ustawienie ID z trasy
            var participantEntryId = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/competitions/{competitionId}/participants/{participantEntryId}", new { id = participantEntryId });
        })
        .WithName("AddParticipantByOrganizer")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequireAuthorization("OrganizerPolicy"); // Wymaga polityki autoryzacji dla Organizatora

        // DELETE /api/competitions/{competitionId}/participants/{participantId}
        // Organizator usuwa uczestnika. Wymaga: Organizator.
        participantsGroup.MapDelete("/{participantId:int}", async (
            int competitionId,
            int participantId,
            ISender mediator,
            CancellationToken ct) =>
        {
            var command = new RemoveParticipantCommand { CompetitionId = competitionId, ParticipantId = participantId };
            await mediator.Send(command, ct);
            return TypedResults.NoContent();
        })
        .WithName("RemoveParticipantByOrganizer")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("OrganizerPolicy");

        // GET /api/competitions/{competitionId}/participants
        // Pobiera listę uczestników danych zawodów.
        participantsGroup.MapGet("/", async (
            int competitionId,
            ISender mediator,
            CancellationToken ct) =>
        {
            var query = new ListCompetitionParticipantsQuery { CompetitionId = competitionId };
            var result = await mediator.Send(query, ct);
            return TypedResults.Ok(result);
        })
        .WithName("ListCompetitionParticipants")
        .Produces<IEnumerable<ParticipantDto>>(StatusCodes.Status200OK);

        // PUT /api/competitions/{competitionId}/participants/{participantId}/assign-judge
        // Organizator nadaje rolę Sędziego. Wymaga: Organizator.
        participantsGroup.MapPut("/{participantId:int}/assign-judge", async (
            int competitionId,
            int participantId,
            ISender mediator,
            CancellationToken ct) =>
        {
            var command = new AssignJudgeRoleCommand { CompetitionId = competitionId, ParticipantId = participantId };
            await mediator.Send(command, ct);
            return TypedResults.Ok();
        })
        .WithName("AssignJudgeRole")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("OrganizerPolicy");

        // PUT /api/competitions/{competitionId}/participants/{participantId}/revoke-judge
        // Organizator odbiera rolę Sędziego. Wymaga: Organizator.
        participantsGroup.MapPut("/{participantId:int}/revoke-judge", async (
            int competitionId,
            int participantId,
            ISender mediator,
            CancellationToken ct) =>
        {
            var command = new RevokeJudgeRoleCommand { CompetitionId = competitionId, ParticipantId = participantId };
            await mediator.Send(command, ct);
            return TypedResults.Ok();
        })
        .WithName("RevokeJudgeRole")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("OrganizerPolicy");

        // --- Fish Catches Sub-Group ---
        var catchesGroup = competitionsGroup.MapGroup("/{competitionId:int}/catches");

        // POST /api/competitions/{competitionId}/catches
        // Sędzia rejestruje złowioną rybę. Wymaga: Sędzia.
        catchesGroup.MapPost("/", async (
            int competitionId,
            RecordFishCatchCommand command, // Command zawiera ParticipantId, SpeciesName, PhotoUrl etc.
            ISender mediator,
            CancellationToken ct) =>
        {
            command.CompetitionId = competitionId;
            var fishCatchId = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/competitions/{competitionId}/catches/{fishCatchId}", new { id = fishCatchId });
        })
        .WithName("RecordFishCatchByJudge")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequireAuthorization("JudgePolicy"); // Wymaga polityki autoryzacji dla Sędziego

        // GET /api/competitions/{competitionId}/catches
        // Pobiera listę zarejestrowanych ryb dla danych zawodów.
        catchesGroup.MapGet("/", async (
            int competitionId,
            ISender mediator,
            CancellationToken ct) =>
        {
            var query = new ListCompetitionCatchesQuery() { CompetitionId = competitionId };
            var result = await mediator.Send(query, ct);
            return TypedResults.Ok(result);
        })
        .WithName("ListCompetitionFishCatches")
        .Produces<IEnumerable<FishCatchDto>>(StatusCodes.Status200OK);


        return app;
    }
}
