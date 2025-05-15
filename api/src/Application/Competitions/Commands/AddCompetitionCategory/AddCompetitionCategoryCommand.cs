namespace Fishio.Application.Competitions.Commands.AddCompetitionCategory;

public record AddCompetitionCategoryCommand : IRequest<int> // Zwraca Id utworzonej CompetitionCategory
{
    public int CompetitionId { get; init; }
    public int CategoryDefinitionId { get; init; }
    public int? SpecificFishSpeciesId { get; init; } // Opcjonalne, jeśli kategoria tego wymaga
    public string? CustomNameOverride { get; init; }
    public string? CustomDescriptionOverride { get; init; }
    public int SortOrder { get; init; } = 0;
    public bool IsPrimaryScoring { get; init; } = false;
    public int MaxWinnersToDisplay { get; init; } = 1;
    public bool IsEnabled { get; init; } = true;
}

public class AddCompetitionCategoryCommandValidator : AbstractValidator<AddCompetitionCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public AddCompetitionCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.CompetitionId)
            .NotEmpty()
            .MustAsync(CompetitionExists).WithMessage("Wybrane zawody nie istnieją.");

        RuleFor(v => v.CategoryDefinitionId)
            .NotEmpty()
            .MustAsync(CategoryDefinitionExists).WithMessage("Wybrana definicja kategorii nie istnieje.");

        RuleFor(v => v.SpecificFishSpeciesId)
            .MustAsync(FishSpeciesExistsOrIsNull).WithMessage("Wybrany gatunek ryby nie istnieje.")
            .When(v => v.SpecificFishSpeciesId.HasValue);

        RuleFor(v => v.MaxWinnersToDisplay)
            .GreaterThanOrEqualTo(1).WithMessage("Liczba wyświetlanych zwycięzców musi być co najmniej 1.");

        // Walidacja, czy SpecificFishSpeciesId jest podane, jeśli CategoryDefinition tego wymaga
        RuleFor(v => v)
            .MustAsync(async (command, cancellation) =>
            {
                var catDef = await _context.CategoryDefinitions
                    .FindAsync(new object[] { command.CategoryDefinitionId }, cancellation);
                if (catDef == null || !catDef.RequiresSpecificFishSpecies)
                {
                    return true; // Nie wymaga gatunku lub definicja nie znaleziona (inny walidator to złapie)
                }
                return command.SpecificFishSpeciesId.HasValue;
            })
            .WithMessage("Ta kategoria wymaga zdefiniowania konkretnego gatunku ryby.")
            .WhenAsync(async (command, cancellation) => // Uruchom tylko jeśli definicja istnieje
            {
                 var catDef = await _context.CategoryDefinitions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cd => cd.Id == command.CategoryDefinitionId, cancellation);
                 return catDef != null && catDef.RequiresSpecificFishSpecies;
            });


        // Walidacja unikalności (CompetitionId, CategoryDefinitionId, SpecificFishSpeciesId)
        RuleFor(v => v)
            .MustAsync(async (command, cancellation) =>
            {
                return !await _context.CompetitionCategories
                    .AnyAsync(cc => cc.CompetitionId == command.CompetitionId &&
                                   cc.CategoryDefinitionId == command.CategoryDefinitionId &&
                                   cc.SpecificFishSpeciesId == command.SpecificFishSpeciesId, // NULL safe comparison
                                   cancellation);
            })
            .WithMessage("Taka kategoria (z tym samym gatunkiem lub bez) już istnieje w tych zawodach.");
    }

    private async Task<bool> CompetitionExists(int competitionId, CancellationToken cancellationToken)
    {
        return await _context.Competitions.AnyAsync(c => c.Id == competitionId, cancellationToken);
    }

    private async Task<bool> CategoryDefinitionExists(int categoryDefinitionId, CancellationToken cancellationToken)
    {
        return await _context.CategoryDefinitions.AnyAsync(cd => cd.Id == categoryDefinitionId, cancellationToken);
    }

    private async Task<bool> FishSpeciesExistsOrIsNull(int? fishSpeciesId, CancellationToken cancellationToken)
    {
        if (!fishSpeciesId.HasValue) return true; // Null jest dozwolony
        return await _context.FishSpecies.AnyAsync(fs => fs.Id == fishSpeciesId.Value, cancellationToken);
    }
}