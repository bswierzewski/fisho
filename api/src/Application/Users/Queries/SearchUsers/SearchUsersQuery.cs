namespace Fishio.Application.Users.Queries.SearchUsers;

public record SearchUsersQuery : IRequest<List<UserDto>>
{
    public string? SearchTerm { get; init; } = null;
    public int? MaxResults { get; init; } = 10;
}

public class SearchUsersQueryValidator : AbstractValidator<SearchUsersQuery>
{
    public SearchUsersQueryValidator()
    {
        RuleFor(x => x.MaxResults)
            .GreaterThan(0).WithMessage("Maksymalna liczba wyników musi być większa niż 0")
            .LessThanOrEqualTo(100).WithMessage("Maksymalna liczba wyników nie może przekraczać 100");
    }
}

public record UserDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? ImageUrl { get; init; }
} 