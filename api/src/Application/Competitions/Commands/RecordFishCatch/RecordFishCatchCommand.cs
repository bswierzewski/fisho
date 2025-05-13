namespace Fishio.Application.Competitions.Commands.RecordFishCatch;

public record RecordFishCatchCommand : IRequest<Unit>
{
    public int CompetitionId { get; init; }
    public string ParticipantId { get; init; } = string.Empty;
    public int FishSpeciesId { get; init; }
    public decimal Length { get; init; }
    public decimal? Weight { get; init; }
    public string? Notes { get; init; }
}

public class RecordFishCatchCommandValidator : AbstractValidator<RecordFishCatchCommand>
{
    public RecordFishCatchCommandValidator()
    {
        RuleFor(x => x.CompetitionId)
            .NotEmpty().WithMessage("Competition ID is required");

        RuleFor(x => x.ParticipantId)
            .NotEmpty().WithMessage("Participant ID is required");

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
    }
} 