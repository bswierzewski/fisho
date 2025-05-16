namespace Fishio.Application.Fisheries.Commands.UpdateFishery;

public record UpdateFisheryCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty; 
    public string? ImageUrl { get; init; }
    public List<int> FishSpeciesIds { get; init; } = new();
}

public class UpdateFisheryCommandValidator : AbstractValidator<UpdateFisheryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateFisheryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id).GreaterThan(0);

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Nazwa łowiska jest wymagana.")
            .MaximumLength(255).WithMessage("Nazwa łowiska nie może przekraczać 255 znaków.");

        // Sprawdzenie unikalności nazwy, ignorując aktualnie edytowane łowisko
        RuleFor(v => v)
            .MustAsync(async (command, cancellation) =>
            {
                return !await _context.Fisheries
                    .AnyAsync(f => f.Name == command.Name && f.Id != command.Id, cancellation);
            })
            .WithMessage("Łowisko o tej nazwie już istnieje.")
            .WithName("Name"); // Aby błąd był przypisany do pola Name

        RuleFor(v => v.Location)
            .MaximumLength(1000).WithMessage("Lokalizacja nie może przekraczać 1000 znaków.");

        RuleFor(v => v.ImageUrl)
            .MaximumLength(2048).WithMessage("URL obrazka nie może przekraczać 2048 znaków.");
    }
} 