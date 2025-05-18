namespace Fishio.Application.Competitions.Commands.UpdateCompetitionCategory;

public class UpdateCompetitionCategoryCommand : IRequest<bool>
{
    public int CompetitionId { get; set; }
    public int CompetitionCategoryId { get; set; } // ID CompetitionCategory do aktualizacji

    // Pola, które można aktualizować
    public string? CustomNameOverride { get; set; }
    public string? CustomDescriptionOverride { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimaryScoring { get; set; } // Uwaga: zmiana tego może być skomplikowana
    public int MaxWinnersToDisplay { get; set; } = 1;
    public bool IsEnabled { get; set; } = true;
    public int? FishSpeciesId { get; set; } // Dla kategorii wymagających konkretnego gatunku
}

public class UpdateCompetitionCategoryCommandValidator : AbstractValidator<UpdateCompetitionCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCompetitionCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.CompetitionId).NotEmpty();
        RuleFor(v => v.CompetitionCategoryId).NotEmpty();

        RuleFor(v => v.CustomNameOverride)
            .MaximumLength(150).WithMessage("Niestandardowa nazwa kategorii nie może przekraczać 150 znaków.");

        RuleFor(v => v.CustomDescriptionOverride)
            .MaximumLength(1000).WithMessage("Niestandardowy opis kategorii nie może przekraczać 1000 znaków.");

        RuleFor(v => v.MaxWinnersToDisplay)
            .GreaterThanOrEqualTo(0).WithMessage("Liczba wyświetlanych zwycięzców nie może być ujemna.");

        When(v => v.FishSpeciesId.HasValue, () => {
            RuleFor(v => v.FishSpeciesId)
                .GreaterThan(0).WithMessage("Nieprawidłowe ID gatunku ryby.")
                .MustAsync(FishSpeciesMustExist).WithMessage("Wybrany gatunek ryby nie istnieje.");
        });

        // Walidacja spójności IsPrimaryScoring
        RuleFor(x => x)
            .MustAsync(BeTheOnlyPrimaryScoringIfSetToTrue)
            .WithMessage("Może istnieć tylko jedna aktywna główna kategoria punktacyjna.")
            .When(x => x.IsPrimaryScoring);

        // Walidacja RequiresSpecificFishSpecies
        RuleFor(x => x)
            .MustAsync(SatisfyFishSpeciesRequirement)
            .WithMessage(cmd => GetFishSpeciesRequirementMessage(cmd.CompetitionCategoryId, cmd.FishSpeciesId.HasValue))
            .WhenAsync(async (cmd, ct) => await CompetitionCategoryExists(cmd.CompetitionCategoryId, ct)); // Sprawdź tylko, jeśli kategoria istnieje
    }

    private async Task<bool> FishSpeciesMustExist(int? fishSpeciesId, CancellationToken cancellationToken)
    {
        if (!fishSpeciesId.HasValue) return true;
        return await _context.FishSpecies.AnyAsync(fs => fs.Id == fishSpeciesId.Value, cancellationToken);
    }

    private async Task<bool> BeTheOnlyPrimaryScoringIfSetToTrue(UpdateCompetitionCategoryCommand command, CancellationToken cancellationToken)
    {
        if (!command.IsPrimaryScoring) return true; // Jeśli nie ustawiamy na true, to jest OK

        // Sprawdź, czy inna kategoria w tych zawodach nie jest już główną
        return !await _context.CompetitionCategories
            .AnyAsync(cc => cc.CompetitionId == command.CompetitionId &&
                             cc.Id != command.CompetitionCategoryId && // Wyklucz aktualizowaną kategorię
                             cc.IsPrimaryScoring &&
                             cc.IsEnabled, cancellationToken);
    }
    private async Task<bool> CompetitionCategoryExists(int competitionCategoryId, CancellationToken cancellationToken)
    {
        return await _context.CompetitionCategories.AnyAsync(cc => cc.Id == competitionCategoryId, cancellationToken);
    }


    private async Task<bool> SatisfyFishSpeciesRequirement(UpdateCompetitionCategoryCommand command, CancellationToken cancellationToken)
    {
        var competitionCategory = await _context.CompetitionCategories
            .Include(cc => cc.CategoryDefinition)
            .FirstOrDefaultAsync(cc => cc.Id == command.CompetitionCategoryId, cancellationToken);

        if (competitionCategory == null) return true; // Walidacja istnienia kategorii jest gdzie indziej

        var definition = competitionCategory.CategoryDefinition;
        if (definition.RequiresSpecificFishSpecies && !command.FishSpeciesId.HasValue)
        {
            return false; // Wymaga gatunku, a nie podano
        }
        if (!definition.RequiresSpecificFishSpecies && command.FishSpeciesId.HasValue)
        {
            return false; // Nie powinna mieć gatunku, a podano
        }
        return true;
    }

    private string GetFishSpeciesRequirementMessage(int competitionCategoryId, bool hasFishSpeciesId)
    {
        // Ta metoda jest trochę skomplikowana do wywołania w kontekście FluentValidation Message,
        // bo potrzebuje dostępu do CategoryDefinition. Można uprościć komunikat
        // lub przenieść tę logikę do handlera.
        // Na razie ogólny komunikat:
        return "Niespełniony wymóg dotyczący gatunku ryby dla tej definicji kategorii.";
    }
}
