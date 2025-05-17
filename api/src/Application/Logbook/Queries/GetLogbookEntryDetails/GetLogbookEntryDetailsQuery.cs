namespace Fishio.Application.Logbook.Queries.GetLogbookEntryDetails;

public record GetLogbookEntryDetailsQuery : IRequest<LogbookEntryDetailsDto>
{
    public int Id { get; init; }
}

public class GetLogbookEntryDetailsQueryValidator : AbstractValidator<GetLogbookEntryDetailsQuery>
{
    public GetLogbookEntryDetailsQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Logbook entry ID is required");
    }
}

public record LogbookEntryDetailsDto
{
    public int Id { get; init; }
    public string ImageUrl { get; set; } = string.Empty;
    public int? FisheryId { get; init; }
    public string? FisheryName { get; init; } = string.Empty;
    public int? FishSpeciesId { get; init; }
    public string? FishSpeciesName { get; init; } = string.Empty;
    public decimal? Length { get; init; }
    public decimal? Weight { get; init; }
    public string? Notes { get; init; }
    public DateTimeOffset CaughtAt { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? LastModifiedAt { get; init; }
}