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
            .GreaterThan(0).WithMessage("Strona musi być większa od 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Strona musi być większa od 0")
            .LessThanOrEqualTo(100).WithMessage("Strona musi być mniejsza lub równa 100");
    }
}