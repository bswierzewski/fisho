namespace Fishio.Application.Competitions.Queries.ListMyCompetitions;

public record ListMyCompetitionsQuery : IRequest<List<MyCompetitionDto>>
{
    public string? Status { get; init; }
    public string? Role { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class ListMyCompetitionsQueryValidator : AbstractValidator<ListMyCompetitionsQuery>
{
    public ListMyCompetitionsQueryValidator()
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

public record MyCompetitionDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public int MyTotalCatches { get; init; }
    public int TotalParticipants { get; init; }
} 