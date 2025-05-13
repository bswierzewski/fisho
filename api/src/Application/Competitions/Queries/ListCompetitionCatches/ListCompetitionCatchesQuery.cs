namespace Fishio.Application.Competitions.Queries.ListCompetitionCatches;

public record ListCompetitionCatchesQuery : IRequest<List<CompetitionCatchDto>>
{
    public int CompetitionId { get; init; }
    public string? ParticipantId { get; init; }
    public int? FishSpeciesId { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class ListCompetitionCatchesQueryValidator : AbstractValidator<ListCompetitionCatchesQuery>
{
    public ListCompetitionCatchesQueryValidator()
    {
        RuleFor(x => x.CompetitionId)
            .NotEmpty().WithMessage("Competition ID is required");

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

public record CompetitionCatchDto
{
    public int Id { get; init; }
    public string ParticipantId { get; init; } = string.Empty;
    public string ParticipantName { get; init; } = string.Empty;
    public int FishSpeciesId { get; init; }
    public string FishSpeciesName { get; init; } = string.Empty;
    public decimal Length { get; init; }
    public decimal? Weight { get; init; }
    public string? Notes { get; init; }
    public DateTime CaughtAt { get; init; }
    public string RecordedByUserId { get; init; } = string.Empty;
    public string RecordedByUserName { get; init; } = string.Empty;
} 