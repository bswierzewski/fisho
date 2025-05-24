﻿using Fishio.Application.Common.Models;
using Fishio.Application.Competitions.Commands.AddParticipant;
using Fishio.Application.Competitions.Commands.AssignJudge;
using Fishio.Application.Competitions.Commands.CancelCompetition;
using Fishio.Application.Competitions.Commands.CreateCompetition;
using Fishio.Application.Competitions.Commands.FinishCompetition;
using Fishio.Application.Competitions.Commands.JoinCompetition;
using Fishio.Application.Competitions.Commands.RecordFishCatch;
using Fishio.Application.Competitions.Commands.RemoveJudge;
using Fishio.Application.Competitions.Commands.RemoveParticipant;
using Fishio.Application.Competitions.Commands.StartCompetition;
using Fishio.Application.Competitions.Commands.UpdateCompetition;
using Fishio.Application.Competitions.Commands.UpdateCompetitionCategory;
using Fishio.Application.Competitions.Queries.GetCompetitionDetails;
using Fishio.Application.Competitions.Queries.GetMyCompetitions;
using Fishio.Application.Competitions.Queries.GetOpenCompetitions;
using Microsoft.AspNetCore.Mvc;

namespace Fishio.API.Endpoints;

public static class CompetitionsEndpoints
{
    public static void MapCompetitionEndpoints(this IEndpointRouteBuilder app)
    {
        var competitionsGroup = app.MapGroup("/api/competitions")
            .WithTags("Competitions")
            .WithOpenApi();

        // --- General Competition Endpoints ---
        competitionsGroup.MapPost("/", CreateNewCompetition)
            .WithName(nameof(CreateNewCompetition))
            .Produces<object>(StatusCodes.Status201Created).ProducesValidationProblem().ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized).ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization()
            .DisableAntiforgery();

        competitionsGroup.MapGet("/open", GetOpenCompetitionsList)
            .WithName(nameof(GetOpenCompetitionsList))
            .Produces<PaginatedList<CompetitionSummaryDto>>(StatusCodes.Status200OK);

        competitionsGroup.MapGet("/my", GetUserCompetitionsList)
            .WithName(nameof(GetUserCompetitionsList))
            .Produces<PaginatedList<CompetitionSummaryDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();

        competitionsGroup.MapGet("/{id:int}", GetCompetitionDetailsById)
            .WithName(nameof(GetCompetitionDetailsById))
            .Produces<CompetitionDetailsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        competitionsGroup.MapPut("/{id:int}", UpdateExistingCompetition)
            .WithName(nameof(UpdateExistingCompetition))
            .Produces(StatusCodes.Status204NoContent).ProducesValidationProblem().ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound).ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization()
            .DisableAntiforgery();

        // --- Status Management ---
        var statusGroup = competitionsGroup.MapGroup("/{competitionId:int}/status")
            .RequireAuthorization(); // Zmiana statusu wymaga autoryzacji (organizator)

        statusGroup.MapPost("/start", OrganizerStartsCompetition)
            .WithName(nameof(OrganizerStartsCompetition))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem().ProducesProblem(StatusCodes.Status400BadRequest) // Dla InvalidOperationException
            .ProducesProblem(StatusCodes.Status404NotFound).ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        statusGroup.MapPost("/finish", OrganizerFinishesCompetition)
            .WithName(nameof(OrganizerFinishesCompetition))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem().ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound).ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        statusGroup.MapPost("/cancel", OrganizerCancelsCompetition)
            .WithName(nameof(OrganizerCancelsCompetition))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem().ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound).ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        // --- Participant Management ---
        var participantsGroup = competitionsGroup.MapGroup("/{competitionId:int}/participants")
            .RequireAuthorization(); // Zarządzanie uczestnikami wymaga autoryzacji

        participantsGroup.MapPost("/join", UserJoinsCompetition) // Użytkownik dołącza sam
            .WithName(nameof(UserJoinsCompetition))
            .Produces<object>(StatusCodes.Status201Created).ProducesValidationProblem().ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound).ProducesProblem(StatusCodes.Status401Unauthorized);

        participantsGroup.MapPost("/", OrganizerAddsParticipant) // Organizator dodaje
            .WithName(nameof(OrganizerAddsParticipant))
            .Produces<object>(StatusCodes.Status201Created).ProducesValidationProblem().ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound).ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        participantsGroup.MapDelete("/{participantEntryId:int}", OrganizerRemovesParticipant)
            .WithName(nameof(OrganizerRemovesParticipant))
            .Produces(StatusCodes.Status204NoContent).ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized).ProducesProblem(StatusCodes.Status403Forbidden);

        // --- Judge Management ---
        var judgesGroup = competitionsGroup.MapGroup("/{competitionId:int}/judges")
            .RequireAuthorization(); // Zarządzanie sędziami wymaga autoryzacji (organizator)

        judgesGroup.MapPost("/", OrganizerAssignsJudge) // Organizator wyznacza sędziego
            .WithName(nameof(OrganizerAssignsJudge))
            .Produces<object>(StatusCodes.Status201Created) // Zwraca { judgeParticipantEntryId = newId }
            .ProducesValidationProblem().ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound).ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        judgesGroup.MapDelete("/{judgeParticipantEntryId:int}", OrganizerRemovesJudge) // Organizator usuwa sędziego
            .WithName(nameof(OrganizerRemovesJudge))
            .Produces(StatusCodes.Status204NoContent).ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized).ProducesProblem(StatusCodes.Status403Forbidden);

        // --- Fish Catch Registration ---
        var catchesGroup = competitionsGroup.MapGroup("/{competitionId:int}/catches")
            .RequireAuthorization(); // Rejestracja połowów wymaga autoryzacji (sędzia)

        catchesGroup.MapPost("/", JudgeRecordsFishCatch)
            .WithName(nameof(JudgeRecordsFishCatch))
            .Produces<object>(StatusCodes.Status201Created) // Zwraca { fishCatchId = newId }
            .ProducesValidationProblem().ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound).ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .DisableAntiforgery();

        // --- Category Management in Competition ---
        var categoryManagementGroup = competitionsGroup.MapGroup("/{competitionId:int}/categories")
            .RequireAuthorization(); // Zarządzanie kategoriami wymaga autoryzacji (organizator)

        categoryManagementGroup.MapPut("/{competitionCategoryId:int}", OrganizerUpdatesCompetitionCategory)
            .WithName(nameof(OrganizerUpdatesCompetitionCategory))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem().ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound).ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);
    }

    // --- Handlers for Endpoints (istniejące i nowe) ---

    private static async Task<IResult> OrganizerUpdatesCompetitionCategory(
        ISender sender,
        int competitionId,
        int competitionCategoryId,
        UpdateCompetitionCategoryCommand command, // Komenda przychodzi z ciała żądania
        CancellationToken ct)
    {
        // Ustawiamy ID z trasy w komendzie, jeśli nie są już zgodne
        if (command.CompetitionId == 0) command.CompetitionId = competitionId;
        else if (command.CompetitionId != competitionId)
            return TypedResults.BadRequest("ID zawodów w ścieżce nie zgadza się z ID w ciele żądania.");

        if (command.CompetitionCategoryId == 0) command.CompetitionCategoryId = competitionCategoryId;
        else if (command.CompetitionCategoryId != competitionCategoryId)
            return TypedResults.BadRequest("ID kategorii w ścieżce nie zgadza się z ID w ciele żądania.");

        await sender.Send(command, ct);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> CreateNewCompetition(ISender sender, [FromForm] CreateCompetitionCommand command, CancellationToken ct)
    {
        var competitionId = await sender.Send(command, ct);
        return TypedResults.Created($"/api/competitions/{competitionId}", new { Id = competitionId });
    }

    private static async Task<IResult> GetOpenCompetitionsList(ISender sender, [AsParameters] GetOpenCompetitionsQuery query, CancellationToken ct)
    {
        var competitions = await sender.Send(query, ct);
        return TypedResults.Ok(competitions);
    }

    private static async Task<IResult> GetUserCompetitionsList(ISender sender, [AsParameters] GetMyCompetitionsQuery query, CancellationToken ct)
    {
        var competitions = await sender.Send(query, ct);
        return TypedResults.Ok(competitions);
    }
    private static async Task<IResult> GetCompetitionDetailsById(ISender sender, int id, CancellationToken ct)
    {
        var query = new GetCompetitionDetailsQuery(id);
        var competition = await sender.Send(query, ct);
        return competition != null ? TypedResults.Ok(competition) : TypedResults.NotFound();
    }

    private static async Task<IResult> UpdateExistingCompetition(ISender sender, int id, [FromForm] UpdateCompetitionCommand command, CancellationToken ct)
    {
        if (id != command.Id) return TypedResults.BadRequest("ID mismatch.");
        await sender.Send(command, ct);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> UserJoinsCompetition(ISender sender, int competitionId, CancellationToken ct)
    {
        var command = new JoinCompetitionCommand { CompetitionId = competitionId };
        var participantEntryId = await sender.Send(command, ct);
        return TypedResults.Created($"/api/competitions/{competitionId}/participants/{participantEntryId}", new { ParticipantEntryId = participantEntryId });
    }

    private static async Task<IResult> OrganizerAddsParticipant(ISender sender, int competitionId, AddParticipantCommand command, CancellationToken ct)
    {
        command.CompetitionId = competitionId; // Ustawiamy ID zawodów z trasy
        var participantEntryId = await sender.Send(command, ct);
        return TypedResults.Created($"/api/competitions/{competitionId}/participants/{participantEntryId}", new { ParticipantEntryId = participantEntryId });
    }

    private static async Task<IResult> OrganizerRemovesParticipant(ISender sender, int competitionId, int participantEntryId, CancellationToken ct)
    {
        var command = new RemoveParticipantCommand { CompetitionId = competitionId, ParticipantEntryId = participantEntryId };
        await sender.Send(command, ct);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> OrganizerAssignsJudge(ISender sender, int competitionId, AssignJudgeCommand command, CancellationToken ct)
    {
        command.CompetitionId = competitionId; // Ustawiamy ID zawodów z trasy
        var judgeParticipantEntryId = await sender.Send(command, ct);
        return TypedResults.Created($"/api/competitions/{competitionId}/judges/{judgeParticipantEntryId}", new { JudgeParticipantEntryId = judgeParticipantEntryId });
    }

    private static async Task<IResult> OrganizerRemovesJudge(ISender sender, int competitionId, int judgeParticipantEntryId, CancellationToken ct)
    {
        var command = new RemoveJudgeCommand { CompetitionId = competitionId, JudgeParticipantEntryId = judgeParticipantEntryId };
        await sender.Send(command, ct);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> JudgeRecordsFishCatch(ISender sender, int competitionId, [FromForm] RecordCompetitionFishCatchCommand command, CancellationToken ct)
    {
        command.CompetitionId = competitionId; // Ustawiamy ID zawodów z trasy
        var fishCatchId = await sender.Send(command, ct);
        return TypedResults.Created($"/api/competitions/{competitionId}/catches/{fishCatchId}", new { FishCatchId = fishCatchId });
    }

    private static async Task<IResult> OrganizerStartsCompetition(ISender sender, int competitionId, CancellationToken ct)
    {
        var command = new StartCompetitionCommand { CompetitionId = competitionId };
        await sender.Send(command, ct);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> OrganizerFinishesCompetition(ISender sender, int competitionId, CancellationToken ct)
    {
        var command = new FinishCompetitionCommand { CompetitionId = competitionId };
        await sender.Send(command, ct);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> OrganizerCancelsCompetition(ISender sender, int competitionId, CancelCompetitionCommand command, CancellationToken ct)
    {
        // Komenda CancelCompetitionCommand przyjmuje Reason w ciele, więc przekazujemy ją bezpośrednio.
        // Upewniamy się, że ID z trasy jest zgodne.
        if (competitionId != command.CompetitionId)
        {
            // Jeśli command.CompetitionId nie jest ustawione, można je ustawić z trasy
            // lub zwrócić błąd, jeśli jest ustawione i różne.
            // Dla prostoty, jeśli nie jest ustawione, ustawiamy je.
            // Jeśli jest ustawione i różne, można by zwrócić BadRequest.
            if (command.CompetitionId == 0) command.CompetitionId = competitionId;
            else return TypedResults.BadRequest("ID zawodów w ścieżce nie zgadza się z ID w ciele żądania.");
        }
        await sender.Send(command, ct);
        return TypedResults.NoContent();
    }
}
