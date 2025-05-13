// Fishio.Api/Endpoints/AboutEndpoints.cs
using Fishio.Application.About.Queries; // Załóżmy, że masz takie zapytanie
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Fishio.API.Endpoints;

public static class About
{
    public static IEndpointRouteBuilder MapAboutEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/about").WithTags("About");

        // GET /api/about
        // Pobiera informacje o wersji i buildzie aplikacji.
        group.MapGet("/", async (ISender mediator, CancellationToken ct) =>
        {
            var query = new GetApplicationInfoQuery();
            var result = await mediator.Send(query, ct);
            return TypedResults.Ok(result);
        })
        .WithName("GetApplicationInfo")
        .Produces<ApplicationInfoDto>(StatusCodes.Status200OK); // ApplicationInfoDto z warstwy Application

        return app;
    }
}
