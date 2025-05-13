namespace Fishio.Application.Logbook.Commands.CreateLogbookEntry;

public record CreateLogbookEntryCommand : IRequest<Unit>
{
    public Guid FisheryId { get; init; }
    public Guid FishSpeciesId { get; init; }
    public decimal Length { get; init; }
    public decimal? Weight { get; init; }
    public string? Notes { get; init; }
    public DateTime CaughtAt { get; init; }
}

public class CreateLogbookEntryCommandValidator : AbstractValidator<CreateLogbookEntryCommand>
{
    public CreateLogbookEntryCommandValidator()
    {
        RuleFor(x => x.FisheryId)
            .NotEmpty().WithMessage("Fishery ID is required");

        RuleFor(x => x.FishSpeciesId)
            .NotEmpty().WithMessage("Fish species ID is required");

        RuleFor(x => x.Length)
            .GreaterThan(0).WithMessage("Length must be greater than 0");

        RuleFor(x => x.Weight)
            .GreaterThan(0).When(x => x.Weight.HasValue)
            .WithMessage("Weight must be greater than 0 when provided");

        RuleFor(x => x.Notes)
            .MaximumLength(500).When(x => x.Notes != null)
            .WithMessage("Notes cannot exceed 500 characters");

        RuleFor(x => x.CaughtAt)
            .NotEmpty().WithMessage("Caught at date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Caught at date cannot be in the future");
    }
} 