using Fishio.Application.Common.Exceptions;

namespace Fishio.Application.Fisheries.Commands.DeleteFishery;

public class DeleteFisheryCommandHandler : IRequestHandler<DeleteFisheryCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    // private readonly IImageStorageService _imageStorageService; // Jeśli chcesz usuwać zdjęcie

    public DeleteFisheryCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeleteFisheryCommand request, CancellationToken cancellationToken)
    {
        var fisheryToDelete = await _context.Fisheries
            .Include(f => f.Competitions) // Sprawdź, czy nie ma powiązanych zawodów
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (fisheryToDelete == null)
        {
            throw new NotFoundException(nameof(Fishery), request.Id.ToString());
        }

        var currentUser = await _currentUserService.GetOrProvisionDomainUserAsync(cancellationToken);
        if (fisheryToDelete.UserId != currentUser?.Id /* && !currentUser.IsAdmin */)
        {
            throw new ForbiddenAccessException();
        }

        // Sprawdzenie reguł biznesowych przed usunięciem
        if (fisheryToDelete.Competitions.Any())
        {
            // Zamiast rzucać wyjątek, można zwrócić Result.Failure lub podobny mechanizm
            throw new InvalidOperationException("Nie można usunąć łowiska, ponieważ są z nim powiązane zawody. Najpierw usuń lub odłącz zawody.");
        }

        // Opcjonalnie: usuń zdjęcie z IImageStorageService, jeśli istnieje
        // if (!string.IsNullOrEmpty(fisheryToDelete.ImageUrl))
        // {
        //     await _imageStorageService.DeleteImageAsync(fisheryToDelete.ImagePublicId);
        // }

        _context.Fisheries.Remove(fisheryToDelete);
        var result = await _context.SaveChangesAsync(cancellationToken);

        return result > 0;
    }
}
