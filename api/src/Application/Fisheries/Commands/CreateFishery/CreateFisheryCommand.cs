namespace Fishio.Application.Fisheries.Commands.CreateFishery;

public record CreateFisheryCommand : IRequest<int>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty; 
    public string? ImageUrl { get; init; }
    public List<int> FishSpeciesIds { get; init; } = new();
}

public class CreateFisheryCommandValidator : AbstractValidator<CreateFisheryCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateFisheryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Nazwa łowiska jest wymagana.")
            .MaximumLength(255).WithMessage("Nazwa łowiska nie może przekraczać 255 znaków.")
            .MustAsync(BeUniqueName).WithMessage("Łowisko o tej nazwie już istnieje.");

        RuleFor(v => v.Location)
            .MaximumLength(1000).WithMessage("Lokalizacja nie może przekraczać 1000 znaków.");

        RuleFor(v => v.ImageUrl)
            .MaximumLength(2048).WithMessage("URL obrazka nie może przekraczać 2048 znaków.");
        // Można dodać walidację formatu URL

        RuleForEach(v => v.FishSpeciesIds)
            .GreaterThan(0).WithMessage("Id gatunku ryby musi być dodatnie.")
            .MustAsync(FishSpeciesExists).WithMessage("Jeden z podanych gatunków ryb nie istnieje.")
            .When(v => v.FishSpeciesIds != null && v.FishSpeciesIds.Any());
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _context.Fisheries.AnyAsync(f => f.Name == name, cancellationToken);
    }

    private async Task<bool> FishSpeciesExists(int fishSpeciesId, CancellationToken cancellationToken)
    {
        return await _context.FishSpecies.AnyAsync(fs => fs.Id == fishSpeciesId, cancellationToken);
    }
}