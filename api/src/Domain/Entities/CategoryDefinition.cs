namespace Fishio.Domain.Entities;

public class CategoryDefinition : BaseAuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsGlobal { get; private set; } // Czy jest to predefiniowana globalna kategoria
    public CategoryType Type { get; private set; }
    public CategoryMetric Metric { get; private set; }
    public CategoryCalculationLogic CalculationLogic { get; private set; }
    public CategoryEntityType EntityType { get; private set; }
    public bool RequiresSpecificFishSpecies { get; private set; }
    public bool AllowManualWinnerAssignment { get; private set; }

    public virtual ICollection<CompetitionCategory> CompetitionCategories { get; private set; } = [];
    
    // Prywatny konstruktor dla EF Core
    private CategoryDefinition() { }

    public CategoryDefinition(
        string name,
        CategoryType type,
        CategoryMetric metric,
        CategoryCalculationLogic calculationLogic,
        CategoryEntityType entityType,
        bool isGlobal = false,
        string? description = null,
        bool requiresSpecificFishSpecies = false,
        bool allowManualWinnerAssignment = false)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        // Podstawowa walidacja spójności
        if (calculationLogic == CategoryCalculationLogic.ManualAssignment && !allowManualWinnerAssignment)
        {
            // Jeśli logiką jest ręczne przypisanie, to AllowManualWinnerAssignment powinno być true
            // Można to ustawić automatycznie lub rzucić wyjątek
            allowManualWinnerAssignment = true;
        }
        if (metric == CategoryMetric.NotApplicable && calculationLogic != CategoryCalculationLogic.ManualAssignment)
        {
            // Jeśli metryka to N/A, to logika powinna być prawdopodobnie ManualAssignment
            // (chyba że są inne przypadki, np. "Pierwszy uczestnik który się zgłosi")
            // To wymaga głębszej analizy specyficznych przypadków użycia.
        }
        if (requiresSpecificFishSpecies && entityType != CategoryEntityType.FishCatch)
        {
            throw new ArgumentException("Wymóg konkretnego gatunku ryby jest sensowny tylko dla kategorii dotyczących pojedynczego połowu (FishCatch).", nameof(requiresSpecificFishSpecies));
        }


        Name = name;
        Description = description;
        IsGlobal = isGlobal;
        Type = type;
        Metric = metric;
        CalculationLogic = calculationLogic;
        EntityType = entityType;
        RequiresSpecificFishSpecies = requiresSpecificFishSpecies;
        AllowManualWinnerAssignment = allowManualWinnerAssignment;
    }

    public void UpdateDefinition(
        string name,
        CategoryType type,
        CategoryMetric metric,
        CategoryCalculationLogic calculationLogic,
        CategoryEntityType entityType,
        string? description,
        bool requiresSpecificFishSpecies,
        bool allowManualWinnerAssignment
        /* User modifyingUser - do walidacji uprawnień, np. tylko admin dla globalnych */)
    {
        // TODO: Dodać walidację uprawnień, szczególnie jeśli IsGlobal == true
        if (IsGlobal /* && !modifyingUser.IsAdmin() */)
        {
            throw new InvalidOperationException("Globalne definicje kategorii mogą być modyfikowane tylko przez administratora.");
        }

        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        // Ponowna walidacja spójności jak w konstruktorze
        if (calculationLogic == CategoryCalculationLogic.ManualAssignment && !allowManualWinnerAssignment)
        {
            allowManualWinnerAssignment = true;
        }
        if (requiresSpecificFishSpecies && entityType != CategoryEntityType.FishCatch)
        {
            throw new ArgumentException("Wymóg konkretnego gatunku ryby jest sensowny tylko dla kategorii dotyczących pojedynczego połowu (FishCatch).", nameof(requiresSpecificFishSpecies));
        }

        Name = name;
        Description = description;
        Type = type;
        Metric = metric;
        CalculationLogic = calculationLogic;
        EntityType = entityType;
        RequiresSpecificFishSpecies = requiresSpecificFishSpecies;
        AllowManualWinnerAssignment = allowManualWinnerAssignment;

        // AddDomainEvent(new CategoryDefinitionUpdatedEvent(this));
    }

    // Metoda pomocnicza do sprawdzania, czy definicja jest odpowiednia dla danego połowu
    public bool IsApplicableToFishCatch(CompetitionFishCatch fishCatch)
    {
        Guard.Against.Null(fishCatch, nameof(fishCatch));

        if (EntityType != CategoryEntityType.FishCatch && EntityType != CategoryEntityType.ParticipantAggregateCatches)
        {
            return false; // Ta definicja nie dotyczy bezpośrednio połowów
        }

        if (RequiresSpecificFishSpecies)
        {
            // Wymaga, aby CompetitionCategory powiązana z tą definicją miała ustawiony FishSpeciesId
            // Tutaj sprawdzamy tylko, czy definicja *może* wymagać gatunku.
            // Faktyczne dopasowanie będzie w logice rankingu, która bierze pod uwagę CompetitionCategory.FishSpeciesId
            return true; // Dalsza weryfikacja w CompetitionCategory
        }
        return true;
    }
}
