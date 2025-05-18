using Fishio.Application.Common.Exceptions;

namespace Fishio.Application.Competitions.Commands.UpdateCompetitionCategory;

public class UpdateCompetitionCategoryCommandHandler : IRequestHandler<UpdateCompetitionCategoryCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateCompetitionCategoryCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(UpdateCompetitionCategoryCommand request, CancellationToken cancellationToken)
    {
        var competition = await _context.Competitions
            .Include(c => c.Categories).ThenInclude(cat => cat.CategoryDefinition) // Potrzebne do walidacji i aktualizacji
            .FirstOrDefaultAsync(c => c.Id == request.CompetitionId, cancellationToken);

        if (competition == null)
        {
            throw new NotFoundException(nameof(Competition), request.CompetitionId.ToString());
        }

        var organizerId = _currentUserService.UserId;
        if (competition.OrganizerId != organizerId)
        {
            throw new ForbiddenAccessException("Tylko organizator może modyfikować kategorie zawodów.");
        }

        // Sprawdzenie, czy można edytować kategorie (np. zawody nie trwają)
        if (!competition.CanModifyDetails()) // Używamy metody domenowej
        {
            throw new InvalidOperationException($"Nie można modyfikować kategorii dla zawodów w statusie '{competition.Status}'.");
        }

        var categoryToUpdate = competition.Categories
            .FirstOrDefault(c => c.Id == request.CompetitionCategoryId);

        if (categoryToUpdate == null)
        {
            throw new NotFoundException(nameof(CompetitionCategory), request.CompetitionCategoryId.ToString());
        }

        // Jeśli ustawiamy tę kategorię jako główną, upewnij się, że inne nie są główne
        if (request.IsPrimaryScoring && !categoryToUpdate.IsPrimaryScoring)
        {
            var otherPrimaryCategories = competition.Categories
                .Where(c => c.Id != categoryToUpdate.Id && c.IsPrimaryScoring && c.IsEnabled)
                .ToList();
            if (otherPrimaryCategories.Any())
            {
                // Można by je automatycznie odznaczyć lub rzucić błąd - rzucamy błąd, walidator też to sprawdza
                throw new InvalidOperationException("Inna kategoria jest już ustawiona jako główna. Najpierw ją odznacz.");
            }
        }

        // Walidacja RequiresSpecificFishSpecies (powtórzenie z walidatora dla pewności w handlerze)
        var definition = categoryToUpdate.CategoryDefinition;
        if (definition.RequiresSpecificFishSpecies && !request.FishSpeciesId.HasValue)
        {
            throw new System.ComponentModel.DataAnnotations.ValidationException($"Kategoria '{definition.Name}' wymaga zdefiniowania konkretnego gatunku ryby.");
        }
        if (!definition.RequiresSpecificFishSpecies && request.FishSpeciesId.HasValue)
        {
            throw new System.ComponentModel.DataAnnotations.ValidationException($"Kategoria '{definition.Name}' nie powinna mieć zdefiniowanego gatunku ryby.");
        }


        // Używamy metody domenowej CompetitionCategory.UpdateConfiguration
        categoryToUpdate.UpdateConfiguration(
            isPrimaryScoring: request.IsPrimaryScoring,
            sortOrder: request.SortOrder,
            maxWinnersToDisplay: request.MaxWinnersToDisplay,
            fishSpeciesId: request.FishSpeciesId,
            customNameOverride: request.CustomNameOverride,
            customDescriptionOverride: request.CustomDescriptionOverride
        );

        if (request.IsEnabled) categoryToUpdate.Enable();
        else categoryToUpdate.Disable();


        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
