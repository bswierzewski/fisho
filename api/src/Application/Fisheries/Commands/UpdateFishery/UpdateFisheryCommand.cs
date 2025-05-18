using Microsoft.AspNetCore.Http; // Dla IFormFile

namespace Fishio.Application.Fisheries.Commands.UpdateFishery;

// Używamy class dla komendy, aby umożliwić np. logikę w setterach, jeśli byłaby potrzebna,
// lub jeśli preferujemy tradycyjne klasy dla komend.
// Record byłby też dobrym wyborem, jeśli chcemy prosty nośnik danych.
public class UpdateFisheryCommand : IRequest<bool> // Zwraca bool wskazujący sukces
{
    public int Id { get; set; } // ID łowiska do aktualizacji
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public IFormFile? Image { get; set; } // Nowe zdjęcie (opcjonalne, jeśli null - nie zmieniaj)
    public bool RemoveCurrentImage { get; set; } = false; // Flaga do usunięcia obecnego zdjęcia
    public List<int>? FishSpeciesIds { get; set; } // Pełna lista ID gatunków dla tego łowiska
}

public class UpdateFisheryCommandValidator : AbstractValidator<UpdateFisheryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateFisheryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("ID łowiska jest wymagane.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Nazwa łowiska jest wymagana.")
            .MaximumLength(255).WithMessage("Nazwa łowiska nie może przekraczać 255 znaków.")
            .MustAsync(BeUniqueNameForAnotherFishery).WithMessage("Łowisko o tej nazwie już istnieje.");

        RuleFor(v => v.Location)
            .MaximumLength(1000).WithMessage("Lokalizacja nie może przekraczać 1000 znaków.");

        RuleForEach(x => x.FishSpeciesIds)
            .GreaterThan(0).When(x => x != null).WithMessage("Nieprawidłowe ID gatunku ryby.");

        When(x => x.Image != null, () =>
        {
            RuleFor(x => x.Image)
                .Must(BeAValidImage).WithMessage("Nieprawidłowy format zdjęcia lub za duży plik (max 5MB).");
        });
    }

    private async Task<bool> BeUniqueNameForAnotherFishery(UpdateFisheryCommand command, string name, CancellationToken cancellationToken)
    {
        // Nazwa musi być unikalna LUB musi należeć do aktualizowanego łowiska
        return !await _context.Fisheries
            .AnyAsync(f => f.Id != command.Id && f.Name == name, cancellationToken);
    }

    private bool BeAValidImage(IFormFile? file)
    {
        if (file == null) return true;
        if (file.Length == 0) return true;
        if (file.Length > 5 * 1024 * 1024) return false;
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        return allowedTypes.Contains(file.ContentType.ToLower());
    }
}
