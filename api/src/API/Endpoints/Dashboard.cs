// Fishio.Api/Endpoints/DashboardEndpoints.cs
using Fishio.Application.Dashboard.Queries; // Załóżmy, że masz takie zapytanie
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Fishio.API.Endpoints;

public static class Dashboard
{
    public static IEndpointRouteBuilder MapDashboardEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dashboard").WithTags("Dashboard");

        // GET /api/dashboard
        // Pobiera zagregowane dane dla dashboardu. Wymaga: Zalogowany Użytkownik.
        group.MapGet("/", async (ISender mediator, CancellationToken ct) =>
        {
            var query = new GetDashboardDataQuery(); // Zakłada pobranie UserId z Claims
            var result = await mediator.Send(query, ct);
            return TypedResults.Ok(result);
        })
        .WithName("GetDashboardData")
        .Produces<DashboardDto>(StatusCodes.Status200OK) // DashboardDto z warstwy Application
        .RequireAuthorization();

        return app;
    }
}
