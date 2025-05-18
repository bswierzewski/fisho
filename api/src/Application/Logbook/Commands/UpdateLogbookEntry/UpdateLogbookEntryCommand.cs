using Microsoft.AspNetCore.Http; // Dla IFormFile

namespace Fishio.Application.LogbookEntries.Commands.UpdateLogbookEntry;

public class UpdateLogbookEntryCommand : IRequest<bool> // Zwraca bool wskazujący sukces
{
    public int Id { get; set; } // ID wpisu do aktualizacji
    public IFormFile? Image { get; set; } // Nowe zdjęcie (opcjonalne)
    public bool RemoveCurrentImage { get; set; } = false;
    public DateTimeOffset? CatchTime { get; set; }
    public decimal? LengthInCm { get; set; }
    public decimal? WeightInKg { get; set; }
    public string? Notes { get; set; }
    public int? FishSpeciesId { get; set; }
    public int? FisheryId { get; set; }
}

public class UpdateLogbookEntryCommandValidator : AbstractValidator<UpdateLogbookEntryCommand>
{
    public UpdateLogbookEntryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID wpisu jest wymagane.");

        When(x => x.Image != null, () =>
        {
            RuleFor(x => x.Image)
                .Must(BeAValidImage).WithMessage("Nieprawidłowy format zdjęcia lub za duży plik (max 5MB).");
        });

        RuleFor(x => x.CatchTime)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow.AddHours(1))
            .When(x => x.CatchTime.HasValue)
            .WithMessage("Czas połowu nie może być znacznie w przyszłości.");

        When(x => x.LengthInCm.HasValue, () =>
        {
            RuleFor(x => x.LengthInCm)
                .GreaterThan(0).WithMessage("Długość musi być wartością dodatnią.")
                .LessThanOrEqualTo(500).WithMessage("Długość nie może przekraczać 500 cm.");
        });

        When(x => x.WeightInKg.HasValue, () =>
        {
            RuleFor(x => x.WeightInKg)
                .GreaterThan(0).WithMessage("Waga musi być wartością dodatnią.")
                .LessThanOrEqualTo(200).WithMessage("Waga nie może przekraczać 200 kg.");
        });

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notatki nie mogą przekraczać 2000 znaków.");

        When(x => x.FishSpeciesId.HasValue, () =>
        {
            RuleFor(x => x.FishSpeciesId)
                .GreaterThan(0).WithMessage("Nieprawidłowe ID gatunku ryby.");
        });

        When(x => x.FisheryId.HasValue, () =>
        {
            RuleFor(x => x.FisheryId)
                .GreaterThan(0).WithMessage("Nieprawidłowe ID łowiska.");
        });
    }

    private bool BeAValidImage(IFormFile? file)
    {
        if (file == null || file.Length == 0) return true; // Opcjonalne, jeśli null - nie waliduj
        if (file.Length > 5 * 1024 * 1024) return false; // Max 5MB
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        return allowedTypes.Contains(file.ContentType.ToLower());
    }
}
