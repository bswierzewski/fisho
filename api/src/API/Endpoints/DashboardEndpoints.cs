using Fishio.Application.Dashboard.Queries.GetDashboardData;

namespace Fishio.API.Endpoints;

public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dashboard")
            .WithTags("Dashboard")
            .WithOpenApi()
            .RequireAuthorization();

        group.MapGet("/", GetUserDashboardData)
            .WithName(nameof(GetUserDashboardData))
            .Produces<DashboardDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized);
    }

    private static async Task<IResult> GetUserDashboardData(
        ISender sender,
        [AsParameters] GetDashboardDataQuery query,
        CancellationToken ct)
    {
        // Obiekt 'query' zostanie automatycznie wypełniony wartościami
        // z query stringa, np. /api/dashboard?MaxRecentItems=3&MaxFeaturedItems=2
        // Jeśli parametry nie zostaną podane, użyte zostaną wartości domyślne z GetDashboardDataQuery.
        var dashboardData = await sender.Send(query, ct);
        return TypedResults.Ok(dashboardData);
    }
}
