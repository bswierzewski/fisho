using Fishio.Application.Common.Interfaces; // Dla IApplicationDbContext
using Fishio.Domain.Enums; // Dla CompetitionType
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http; // Dla IFormFile
using Microsoft.EntityFrameworkCore; // Dla AnyAsync

namespace Fishio.Application.Competitions.Commands.CreateCompetition;

public class CreateCompetitionCommand : IRequest<int> // Zwraca ID nowych zawodów
{
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public int FisheryId { get; set; } // Lokalizacja będzie pobierana z łowiska
    public string? Rules { get; set; }
    public CompetitionType Type { get; set; } // Otwarte (Public) lub Zamknięte (Private)
    public IFormFile? Image { get; set; } // Opcjonalne zdjęcie zawodów

    // Kategorie
    public int PrimaryScoringCategoryDefinitionId { get; set; } // ID definicji głównej kategorii
    public int? PrimaryScoringFishSpeciesId { get; set; } // Opcjonalne ID gatunku dla głównej kategorii

    public List<SpecialCategoryDefinitionCommandDto>? SpecialCategories { get; set; }
}

public class SpecialCategoryDefinitionCommandDto
{
    public int CategoryDefinitionId { get; set; }
    public int? FishSpeciesId { get; set; } // Opcjonalne ID gatunku dla kategorii specjalnej
    public string? CustomNameOverride { get; set; }
    // Można dodać inne pola do konfiguracji kategorii specjalnej, jeśli potrzebne
}


public class CreateCompetitionCommandValidator : AbstractValidator<CreateCompetitionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly TimeProvider _timeProvider;

    public CreateCompetitionCommandValidator(IApplicationDbContext context, TimeProvider timeProvider)
    {
        _context = context;
        _timeProvider = timeProvider;
        var now = _timeProvider.GetUtcNow();

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Nazwa zawodów jest wymagana.")
            .MaximumLength(250).WithMessage("Nazwa zawodów nie może przekraczać 250 znaków.");
        // Można dodać walidację unikalności nazwy zawodów, jeśli potrzebne

        RuleFor(v => v.StartTime)
            .NotEmpty().WithMessage("Czas rozpoczęcia jest wymagany.")
            .GreaterThan(now.AddMinutes(5)).WithMessage("Czas rozpoczęcia musi być w przyszłości (minimum 5 minut od teraz).");

        RuleFor(v => v.EndTime)
            .NotEmpty().WithMessage("Czas zakończenia jest wymagany.")
            .GreaterThan(v => v.StartTime.AddMinutes(30)) // Zawody muszą trwać minimum 30 minut
            .WithMessage("Czas zakończenia musi być późniejszy niż czas rozpoczęcia (minimum o 30 minut).");

        RuleFor(v => v.FisheryId)
            .NotEmpty().WithMessage("ID łowiska jest wymagane.")
            .GreaterThan(0).WithMessage("Nieprawidłowe ID łowiska.")
            .MustAsync(FisheryMustExist).WithMessage("Wybrane łowisko nie istnieje.");

        RuleFor(v => v.Rules)
            .MaximumLength(10000).WithMessage("Regulamin nie może przekraczać 10000 znaków."); // Duża pojemność

        RuleFor(v => v.Type)
            .IsInEnum().WithMessage("Nieprawidłowy typ zawodów.");

        When(x => x.Image != null, () =>
        {
            RuleFor(x => x.Image)
                .Must(BeAValidImage).WithMessage("Nieprawidłowy format zdjęcia lub za duży plik (max 5MB).");
        });

        // Walidacja kategorii
        RuleFor(v => v.PrimaryScoringCategoryDefinitionId)
            .NotEmpty().WithMessage("ID głównej kategorii punktacyjnej jest wymagane.")
            .GreaterThan(0).WithMessage("Nieprawidłowe ID głównej kategorii punktacyjnej.")
            .MustAsync(CategoryDefinitionMustExist).WithMessage("Wybrana główna kategoria punktacyjna nie istnieje lub nie jest odpowiednia.");

        When(v => v.PrimaryScoringFishSpeciesId.HasValue, () => {
            RuleFor(v => v.PrimaryScoringFishSpeciesId)
                .GreaterThan(0).WithMessage("Nieprawidłowe ID gatunku ryby dla głównej kategorii.")
                .MustAsync(FishSpeciesMustExist).WithMessage("Wybrany gatunek ryby dla głównej kategorii nie istnieje.");
        });


        RuleForEach(v => v.SpecialCategories).ChildRules(category =>
        {
            if (category != null) // FluentValidation może przekazać null dla elementów kolekcji
            {
                category.RuleFor(c => c.CategoryDefinitionId)
                    .NotEmpty().WithMessage("ID definicji kategorii specjalnej jest wymagane.")
                    .GreaterThan(0).WithMessage("Nieprawidłowe ID definicji kategorii specjalnej.")
                    .MustAsync(CategoryDefinitionMustExist).WithMessage("Wybrana definicja kategorii specjalnej nie istnieje lub nie jest odpowiednia.");

                category.When(c => c.FishSpeciesId.HasValue, () => {
                    category.RuleFor(c => c.FishSpeciesId)
                        .GreaterThan(0).WithMessage("Nieprawidłowe ID gatunku ryby dla kategorii specjalnej.")
                        .MustAsync(FishSpeciesMustExist).WithMessage("Wybrany gatunek ryby dla kategorii specjalnej nie istnieje.");
                });

                category.RuleFor(c => c.CustomNameOverride)
                    .MaximumLength(150).WithMessage("Niestandardowa nazwa kategorii nie może przekraczać 150 znaków.");
            }
        }).When(v => v.SpecialCategories != null);
    }

    private async Task<bool> FisheryMustExist(int fisheryId, CancellationToken cancellationToken)
    {
        return await _context.Fisheries.AnyAsync(f => f.Id == fisheryId, cancellationToken);
    }

    private async Task<bool> CategoryDefinitionMustExist(int categoryDefinitionId, CancellationToken cancellationToken)
    {
        // Można dodać bardziej szczegółową walidację, np. czy definicja jest typu 'MainScoring' lub 'SpecialAchievement'
        return await _context.CategoryDefinitions.AnyAsync(cd => cd.Id == categoryDefinitionId, cancellationToken);
    }

    private async Task<bool> FishSpeciesMustExist(int? fishSpeciesId, CancellationToken cancellationToken)
    {
        if (!fishSpeciesId.HasValue) return true; // Opcjonalne
        return await _context.FishSpecies.AnyAsync(fs => fs.Id == fishSpeciesId.Value, cancellationToken);
    }

    private bool BeAValidImage(IFormFile? file)
    {
        if (file == null || file.Length == 0) return true; // Opcjonalne
        if (file.Length > 5 * 1024 * 1024) return false; // Max 5MB
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        return allowedTypes.Contains(file.ContentType.ToLower());
    }
}
