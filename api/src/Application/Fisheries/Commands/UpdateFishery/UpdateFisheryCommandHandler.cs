using Application.Common.Interfaces.Services;
using Fishio.Application.Common.Exceptions;

namespace Fishio.Application.Fisheries.Commands.UpdateFishery;

public class UpdateFisheryCommandHandler : IRequestHandler<UpdateFisheryCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IImageStorageService _imageStorageService;

    public UpdateFisheryCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IImageStorageService imageStorageService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _imageStorageService = imageStorageService;
    }

    public async Task<bool> Handle(UpdateFisheryCommand request, CancellationToken cancellationToken)
    {
        var fisheryToUpdate = await _context.Fisheries
            .Include(f => f.FishSpecies) // Załaduj obecne gatunki do modyfikacji
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (fisheryToUpdate == null)
        {
            throw new NotFoundException(nameof(Fishery), request.Id.ToString());
        }

        var currentUser = await _currentUserService.GetOrProvisionDomainUserAsync(cancellationToken);
        // Sprawdzenie uprawnień: tylko twórca łowiska lub administrator może je edytować
        // (Zakładamy, że administrator miałby specjalną rolę/claim, tu uproszczenie)
        if (fisheryToUpdate.UserId != currentUser?.Id /* && !currentUser.IsAdmin */)
        {
            throw new ForbiddenAccessException();
        }

        string? newImageUrl = fisheryToUpdate.ImageUrl; // Domyślnie stary URL

        if (request.RemoveCurrentImage && !string.IsNullOrEmpty(fisheryToUpdate.ImageUrl))
        {
            // TODO: Implementacja logiki usuwania starego zdjęcia z IImageStorageService
            // Potrzebny byłby PublicId zdjęcia, jeśli Cloudinary go zwraca i jest przechowywany.
            // Na razie zakładamy, że usunięcie oznacza wyczyszczenie ImageUrl.
            // await _imageStorageService.DeleteImageAsync(fisheryToUpdate.ImagePublicId);
            newImageUrl = null;
        }

        if (request.Image != null && request.Image.Length > 0)
        {
            // Jeśli jest nowe zdjęcie, usuń stare (jeśli istniało i nie zostało już usunięte)
            if (!request.RemoveCurrentImage && !string.IsNullOrEmpty(fisheryToUpdate.ImageUrl))
            {
                // await _imageStorageService.DeleteImageAsync(fisheryToUpdate.ImagePublicId);
            }

            ImageUploadResult imageResult;
            await using (var memoryStream = new MemoryStream())
            {
                await request.Image.CopyToAsync(memoryStream, cancellationToken);
                memoryStream.Position = 0;
                imageResult = await _imageStorageService.UploadImageAsync(
                    memoryStream,
                    request.Image.FileName,
                    $"fisheries/{currentUser.Id}"); // lub fisheryToUpdate.Id
            }

            if (!imageResult.Success || string.IsNullOrEmpty(imageResult.Url))
            {
                throw new ApplicationException($"Nie udało się przesłać nowego zdjęcia dla łowiska: {imageResult.ErrorMessage}");
            }
            newImageUrl = imageResult.Url;
        }

        // Użyj metody domenowej do aktualizacji szczegółów
        fisheryToUpdate.UpdateDetails(request.Name, request.Location, newImageUrl);

        // Zarządzanie gatunkami ryb - pełna aktualizacja listy
        // Usuń wszystkie obecne gatunki i dodaj te z request.FishSpeciesIds
        // To jest proste podejście; bardziej zaawansowane mogłoby wykrywać różnice.
        var existingSpecies = fisheryToUpdate.FishSpecies.ToList(); // Kopia do iteracji
        foreach (var species in existingSpecies)
        {
            fisheryToUpdate.RemoveSpecies(species);
        }

        if (request.FishSpeciesIds != null && request.FishSpeciesIds.Any())
        {
            var speciesToAdd = await _context.FishSpecies
                .Where(fs => request.FishSpeciesIds.Contains(fs.Id))
                .ToListAsync(cancellationToken);

            foreach (var species in speciesToAdd)
            {
                fisheryToUpdate.AddSpecies(species);
            }
        }

        // _context.Fisheries.Update(fisheryToUpdate); // Nie jest potrzebne, jeśli encja jest śledzona
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
