using Fishio.Application.Competitions.Commands.AddParticipant;
using Fishio.Application.Competitions.Commands.AssignJudgeRole;
using Fishio.Application.Competitions.Commands.CreateCompetition;
using Fishio.Application.Competitions.Commands.JoinCompetition;
using Fishio.Application.Competitions.Commands.RecordFishCatch;
using Fishio.Application.Competitions.Commands.RemoveParticipant;
using Fishio.Application.Competitions.Queries.GetCompetitionDetails;
using Fishio.Application.Competitions.Queries.ListCompetitionCatches;
using Fishio.Application.Competitions.Queries.ListCompetitionParticipants;
using Fishio.Application.Competitions.Queries.ListCompetitions;
using Fishio.Application.Competitions.Queries.ListMyCompetitions;
using Microsoft.AspNetCore.Mvc;

namespace Fishio.API.Endpoints;

public static class CompetitionEndpoints
{
    public static void MapCompetitionEndpoints(this IEndpointRouteBuilder app)
    {
        var competitionsGroup = app.MapGroup("/api/competitions")
            .WithTags("Competitions")
            .WithOpenApi();

        competitionsGroup.MapPost("/", CreateCompetition)
            .WithName(nameof(CreateCompetition))
            .Produces<object>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .RequireAuthorization();

        competitionsGroup.MapGet("/", ListAvailableCompetitions)
            .WithName(nameof(ListAvailableCompetitions));

        competitionsGroup.MapGet("/{competitionId:int}", GetCompetitionDetailsById)
            .WithName(nameof(GetCompetitionDetailsById));

        competitionsGroup.MapGet("/mine", ListMyCompetitions)
            .WithName(nameof(ListMyCompetitions))
            .RequireAuthorization();

        // Participants
        var participantsGroup = competitionsGroup.MapGroup("/{competitionId:int}/participants")
            .RequireAuthorization();

        participantsGroup.MapPost("/join", JoinCompetitionAsParticipant)
            .WithName(nameof(JoinCompetitionAsParticipant));
        participantsGroup.MapGet("/", ListCompetitionParticipantsForCompetition)
            .WithName(nameof(ListCompetitionParticipantsForCompetition));
        participantsGroup.MapPost("/add-by-organizer", AddParticipantByCompetitionOrganizer)
            .WithName(nameof(AddParticipantByCompetitionOrganizer))
            .RequireAuthorization("OrganizerPolicy");
        participantsGroup.MapDelete("/{participantId:int}", RemoveParticipantByCompetitionOrganizer)
            .WithName(nameof(RemoveParticipantByCompetitionOrganizer))
            .RequireAuthorization("OrganizerPolicy");
        participantsGroup.MapPut("/{participantId:int}/assign-judge", AssignJudgeRoleToParticipant)
            .WithName(nameof(AssignJudgeRoleToParticipant))
            .RequireAuthorization("OrganizerPolicy");

        // Fish Catches
        var catchesGroup = competitionsGroup.MapGroup("/{competitionId:int}/catches")
            .RequireAuthorization();

        catchesGroup.MapPost("/", RecordFishCatchForParticipant)
            .WithName(nameof(RecordFishCatchForParticipant))
            .RequireAuthorization("JudgePolicy");
        catchesGroup.MapGet("/", ListFishCatchesForCompetition)
            .WithName(nameof(ListFishCatchesForCompetition));
    }

    // --- Handlery Metod (prywatne statyczne lub publiczne statyczne) ---
    private static async Task<IResult> CreateCompetition(ISender sender, [FromBody] CreateCompetitionCommand command, CancellationToken ct)
    {
        var competitionId = await sender.Send(command, ct);
        return TypedResults.CreatedAtRoute(nameof(GetCompetitionDetailsById), competitionId.ToString(), new { id = competitionId });
    }

    private static async Task<IResult> ListAvailableCompetitions(ISender sender, [AsParameters] ListCompetitionsQuery query, CancellationToken ct)
    {
        var competitions = await sender.Send(query, ct);
        return TypedResults.Ok(competitions);
    }

    private static async Task<IResult> GetCompetitionDetailsById(ISender sender, [FromRoute] int competitionId, CancellationToken ct)
    {
        var query = new GetCompetitionDetailsQuery { CompetitionId = competitionId };
        var competition = await sender.Send(query, ct);
        return competition != null ? TypedResults.Ok(competition) : TypedResults.NotFound();
    }

    private static async Task<IResult> ListMyCompetitions(ISender sender, CancellationToken ct)
    {
        var query = new ListMyCompetitionsQuery();
        var competitions = await sender.Send(query, ct);
        return TypedResults.Ok(competitions);
    }

    private static async Task<IResult> JoinCompetitionAsParticipant(ISender sender, [FromRoute] int competitionId, CancellationToken ct)
    {
        var command = new JoinCompetitionCommand { CompetitionId = competitionId };
        await sender.Send(command, ct);
        return TypedResults.Ok(new { Message = "Successfully joined competition." });
    }

    private static async Task<IResult> ListCompetitionParticipantsForCompetition(ISender sender, [FromRoute] int competitionId, CancellationToken ct)
    {
        var query = new ListCompetitionParticipantsQuery { CompetitionId = competitionId };
        var participants = await sender.Send(query, ct);
        return TypedResults.Ok(participants);
    }

    private static async Task<IResult> AddParticipantByCompetitionOrganizer(ISender sender, [FromRoute] int competitionId, AddParticipantCommand command, CancellationToken ct)
    {
        var participantEntryId = await sender.Send(command, ct);
        return TypedResults.Created($"/api/competitions/{competitionId}/participants/{participantEntryId}", new { id = participantEntryId });
    }

    private static async Task<IResult> RemoveParticipantByCompetitionOrganizer(ISender sender, [FromRoute] int competitionId, [FromRoute] int participantId, CancellationToken ct)
    {
        var command = new RemoveParticipantCommand { CompetitionId = competitionId, ParticipantId = participantId };
        await sender.Send(command, ct);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> AssignJudgeRoleToParticipant(ISender sender, [FromRoute] int competitionId, [FromRoute] int participantId, CancellationToken ct)
    {
        var command = new AssignJudgeRoleCommand { CompetitionId = competitionId, ParticipantId = participantId };
        await sender.Send(command, ct);
        return TypedResults.Ok(new { Message = "Role assigned." });
    }

    private static async Task<IResult> RecordFishCatchForParticipant(ISender sender, [FromRoute] int competitionId, [FromBody] RecordFishCatchCommand command, CancellationToken ct)
    {
        var fishCatchId = await sender.Send(command, ct);
        return TypedResults.Created($"/api/competitions/{competitionId}/catches/{fishCatchId}", new { id = fishCatchId });
    }

    private static async Task<IResult> ListFishCatchesForCompetition(ISender sender, [FromRoute] int competitionId, CancellationToken ct)
    {
        var query = new ListCompetitionCatchesQuery { CompetitionId = competitionId };
        var catches = await sender.Send(query, ct);
        return TypedResults.Ok(catches);
    }
}
