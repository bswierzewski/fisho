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

        group.MapGet("/", GetDashboardInformation)
            .WithName(nameof(GetDashboardInformation));
    }

    private static async Task<IResult> GetDashboardInformation(ISender sender, CancellationToken ct)
    {
        var query = new GetDashboardDataQuery();
        var dashboardData = await sender.Send(query, ct);
        return TypedResults.Ok(dashboardData);
    }
}
