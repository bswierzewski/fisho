namespace Fishio.Application.LogbookEntries.Queries.GetLogbookEntryById;

// DTO LogbookEntryDto jest już zdefiniowane w Fishio.Application.LogbookEntries.Queries
// using Fishio.Application.LogbookEntries.Queries;

public class GetLogbookEntryByIdQuery : IRequest<LogbookEntryDto?>
{
    public int Id { get; set; }

    public GetLogbookEntryByIdQuery(int id)
    {
        Id = id;
    }
}

public class GetLogbookEntryByIdQueryValidator : AbstractValidator<GetLogbookEntryByIdQuery>
{
    public GetLogbookEntryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Logbook entry ID is required");
    }
}

// Możemy użyć record dla DTO, jeśli preferujemy zwięzłość i niemutowalność
public record LogbookEntryDto
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