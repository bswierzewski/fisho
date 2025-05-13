namespace Fishio.Application.Fisheries.Commands.UpdateFishery;

public record UpdateFisheryCommand : IRequest<Unit>
{
    public Guid FisheryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public List<Guid> FishSpeciesIds { get; init; } = new();
}

public class UpdateFisheryCommandValidator : AbstractValidator<UpdateFisheryCommand>
{
    public UpdateFisheryCommandValidator()
    {
        RuleFor(x => x.FisheryId)
            .NotEmpty().WithMessage("Fishery ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required")
            .MaximumLength(200).WithMessage("Location cannot exceed 200 characters");

        RuleFor(x => x.FishSpeciesIds)
            .NotEmpty().WithMessage("At least one fish species must be selected");

        RuleForEach(x => x.FishSpeciesIds)
            .NotEmpty().WithMessage("Fish species ID cannot be empty");
    }
} 