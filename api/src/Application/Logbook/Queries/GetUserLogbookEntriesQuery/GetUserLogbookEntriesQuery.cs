namespace Fishio.Application.Logbook.Queries.ListLogbookEntries;

public class GetUserLogbookEntriesQuery : IRequest<PaginatedList<UserLogbookEntryDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    // Można dodać filtry, np. po gatunku, dacie
}

public class ListLogbookEntriesQueryValidator : AbstractValidator<GetUserLogbookEntriesQuery>
{
    public ListLogbookEntriesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Strona musi być większa od 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Strona musi być większa od 0")
            .LessThanOrEqualTo(100).WithMessage("Strona musi być mniejsza lub równa 100");
    }
}

// Możemy użyć record dla DTO, jeśli preferujemy zwięzłość i niemutowalność
public record UserLogbookEntryDto
{
    public int Id { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public DateTimeOffset CatchTime { get; init; }
    public decimal? LengthInCm { get; init; }
    public decimal? WeightInKg { get; init; }
    public string? Notes { get; init; }
    public int? FishSpeciesId { get; init; }
    public string? FishSpeciesName { get; init; } // Nazwa gatunku dla wyświetlania
    public int? FisheryId { get; init; }
    public string? FisheryName { get; init; } // Nazwa łowiska dla wyświetlania
    public DateTimeOffset Created { get; init; }
}