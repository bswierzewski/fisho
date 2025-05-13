// Fishio.Api/Endpoints/UserEndpoints.cs
using Fishio.Application.Users.Queries.SearchUsers;
// DTOs z warstwy Application
using Fishio.Application.Users.Queries; // np. UserSearchResultDto
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace Fishio.API.Endpoints;

public static class User
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users").RequireAuthorization();

        // GET /api/users/search?query=kowal
        // Wyszukuje zarejestrowanych użytkowników.
        group.MapGet("/search", async (
            [FromQuery] string query,
            ISender mediator,
            CancellationToken ct) =>
        {
            var searchQuery = new SearchUsersQuery { Query = query };
            var result = await mediator.Send(searchQuery, ct);
            return TypedResults.Ok(result);
        })
        .WithName("SearchUsers")
        .Produces<IEnumerable<UserSearchResultDto>>(StatusCodes.Status200OK);

        // Profil użytkownika jest zarządzany przez Clerk na /profile/[[...user-profile]],
        // więc nie ma tu dedykowanego endpointu API do pobierania/edycji profilu,
        // chyba że potrzebujesz specyficznych danych z Twojej bazy dla profilu.

        return app;
    }
}
