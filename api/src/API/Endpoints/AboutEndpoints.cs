using Fishio.Application.About.Queries.GetApplicationInfo;

namespace Fishio.API.Endpoints;

public static class AboutEndpoints
{
    // Metoda mapująca endpointy dla tej grupy
    public static void MapAboutEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/about")
            .WithTags("About")
            .WithOpenApi();

        group.MapGet("/", GetApplicationInformation)
            .WithName(nameof(GetApplicationInformation))
            .Produces<ApplicationInfoDto>(StatusCodes.Status200OK);
    }

    // Handler dla GET /api/about/
    private static async Task<IResult> GetApplicationInformation(ISender sender, CancellationToken ct)
    {
        var query = new GetApplicationInfoQuery();
        var result = await sender.Send(query, ct);
        return TypedResults.Ok(result);
    }
}
