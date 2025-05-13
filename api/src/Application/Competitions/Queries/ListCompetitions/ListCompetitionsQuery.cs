namespace Fishio.Application.Competitions.Queries.ListCompetitions;

public record ListCompetitionsQuery : IRequest<List<CompetitionDto>>
{
    public string? SearchTerm { get; init; }
    public CompetitionStatus? Status { get; init; }
    public CompetitionType? Type { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class ListCompetitionsQueryValidator : AbstractValidator<ListCompetitionsQuery>
{
    public ListCompetitionsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100");

        RuleFor(x => x.FromDate)
            .LessThanOrEqualTo(x => x.ToDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
            .WithMessage("From date must be less than or equal to To date");
    }
}

public record CompetitionDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string OrganizerId { get; init; } = string.Empty;
    public string OrganizerName { get; init; } = string.Empty;
    public int ParticipantsCount { get; init; }
} 