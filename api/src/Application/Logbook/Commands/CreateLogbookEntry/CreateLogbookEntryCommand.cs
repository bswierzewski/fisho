namespace Fishio.Application.Logbook.Commands.CreateLogbookEntry;

public record CreateLogbookEntryCommand : IRequest<int>
{
    public string? ImageUrl { get; init; }
    public int? FisheryId { get; init; }
    public int? FishSpeciesId { get; init; }
    public decimal? Length { get; init; }
    public decimal? Weight { get; init; }
    public string? Notes { get; init; }
    public DateTimeOffset? CaughtAt { get; init; }
}

public class CreateLogbookEntryCommandValidator : AbstractValidator<CreateLogbookEntryCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateLogbookEntryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Notes)
            .MaximumLength(2000).WithMessage("Notatki nie mogą przekraczać 2000 znaków.");
    }
}
