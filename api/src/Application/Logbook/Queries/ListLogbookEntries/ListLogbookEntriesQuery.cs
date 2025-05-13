namespace Fishio.Application.Logbook.Queries.ListLogbookEntries;

public record ListLogbookEntriesQuery : IRequest<List<LogbookEntryDto>>
{
    public int? FisheryId { get; init; }
    public int? FishSpeciesId { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class ListLogbookEntriesQueryValidator : AbstractValidator<ListLogbookEntriesQuery>
{
    public ListLogbookEntriesQueryValidator()
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

public record LogbookEntryDto
{
    public int Id { get; init; }
    public int FisheryId { get; init; }
    public string FisheryName { get; init; } = string.Empty;
    public int FishSpeciesId { get; init; }
    public string FishSpeciesName { get; init; } = string.Empty;
    public decimal Length { get; init; }
    public decimal? Weight { get; init; }
    public string? Notes { get; init; }
    public DateTime CaughtAt { get; init; }
} 