using Fishio.Application.Users.Queries.SearchUsers;

namespace Fishio.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();

        group.MapGet("/search", SearchAvailableUsers)
            .WithName(nameof(SearchAvailableUsers));
    }

    private static async Task<IResult> SearchAvailableUsers(ISender sender, [AsParameters] SearchUsersQuery query, CancellationToken ct)
    {
        var users = await sender.Send(query, ct);
        return TypedResults.Ok(users);
    }
}
