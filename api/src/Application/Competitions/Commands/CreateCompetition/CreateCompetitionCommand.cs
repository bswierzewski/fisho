namespace Fishio.Application.Competitions.Commands.CreateCompetition;

public record CreateCompetitionCommand : IRequest<Unit>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Type { get; init; } = string.Empty;
    public List<Guid> FishSpeciesIds { get; init; } = new();
    public List<string> JudgeIds { get; init; } = new();
}

public class CreateCompetitionCommandValidator : AbstractValidator<CreateCompetitionCommand>
{
    public CreateCompetitionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("Start date must be in the future");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Competition type is required");

        RuleFor(x => x.FishSpeciesIds)
            .NotEmpty().WithMessage("At least one fish species must be selected");

        RuleForEach(x => x.FishSpeciesIds)
            .NotEmpty().WithMessage("Fish species ID cannot be empty");

        RuleForEach(x => x.JudgeIds)
            .NotEmpty().WithMessage("Judge ID cannot be empty");
    }
}
