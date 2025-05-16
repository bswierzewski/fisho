namespace Fishio.Application.Fisheries.Queries.ListFisheries;

public record ListFisheriesQuery : IRequest<PaginatedList<FisheryDto>>
{
    public string? SearchTerm { get; init; }
    public int? FishSpeciesId { get; init; }
    public int? UserId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class ListFisheriesQueryValidator : AbstractValidator<ListFisheriesQuery>
{
    public ListFisheriesQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100");
    }
}

public record FisheryDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? Location { get; init; }
    public string? ImageUrl { get; set; }
    public int FishSpeciesCount { get; init; }
    public int TotalCatchesCount { get; init; }
    public DateTime? LastCatchDate { get; init; }
} 