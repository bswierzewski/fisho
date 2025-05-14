namespace Fishio.Application.Users.Queries.SearchUsers;

public record SearchUsersQuery : IRequest<List<UserDto>>
{
    public string SearchTerm { get; init; } = string.Empty;
    public int MaxResults { get; init; } = 10;
}

public class SearchUsersQueryValidator : AbstractValidator<SearchUsersQuery>
{
    public SearchUsersQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
            .NotEmpty().WithMessage("Search term is required")
            .MinimumLength(2).WithMessage("Search term must be at least 2 characters long");

        RuleFor(x => x.MaxResults)
            .GreaterThan(0).WithMessage("Max results must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Max results cannot exceed 100");
    }
}

public record UserDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? ImageUrl { get; init; }
} 