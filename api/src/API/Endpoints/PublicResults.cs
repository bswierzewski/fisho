// Fishio.Api/Endpoints/PublicResultsEndpoints.cs
using Fishio.Application.PublicResults.Queries; // Załóżmy, że masz takie zapytanie
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Fishio.API.Endpoints;

public static class PublicResults
{
    public static IEndpointRouteBuilder MapPublicResultsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/results").WithTags("PublicResults");

        // GET /api/results/{token}
        // Pobiera publiczne wyniki zawodów na podstawie tokenu.
        group.MapGet("/{token}", async (
            string token,
            ISender mediator,
            CancellationToken ct) =>
        {
            var query = new GetPublicCompetitionResultsQuery { ResultsToken = token };
            var result = await mediator.Send(query, ct);
            return result != null ? TypedResults.Ok(result) : TypedResults.NotFound();
        })
        .WithName("GetPublicCompetitionResults")
        .Produces<PublicCompetitionResultsDto>(StatusCodes.Status200OK) // PublicCompetitionResultsDto z Application
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
