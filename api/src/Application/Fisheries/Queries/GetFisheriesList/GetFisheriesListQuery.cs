namespace Fishio.Application.Fisheries.Queries.ListFisheries;

public record GetFisheriesListQuery : IRequest<PaginatedList<FisheryDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; }
}

public class ListFisheriesQueryValidator : AbstractValidator<GetFisheriesListQuery>
{
    public ListFisheriesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Strona musi być większa od 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Strona musi być większa od 0")
            .LessThanOrEqualTo(100).WithMessage("Strona musi być mniejsza lub równa 100");
    }
}

public class FisheryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Location { get; set; }
    public int? UserId { get; set; } // Creator/Maintainer
    public string? UserName { get; set; } // Imię twórcy/opiekuna
    public DateTimeOffset Created { get; set; }
    public ICollection<FishSpeciesSimpleDto> FishSpecies { get; set; } = new List<FishSpeciesSimpleDto>();
}

public class FishSpeciesSimpleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}