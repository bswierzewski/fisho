namespace Fishio.Application.Fisheries.Commands.CreateFishery;

public record CreateFisheryCommand : IRequest<int>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public List<int> FishSpeciesIds { get; init; } = new();
}

public class CreateFisheryCommandValidator : AbstractValidator<CreateFisheryCommand>
{
    public CreateFisheryCommandValidator()
    {
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