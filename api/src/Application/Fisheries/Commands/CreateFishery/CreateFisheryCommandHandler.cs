using Application.Common.Interfaces.Services;

namespace Fishio.Application.Fisheries.Commands.CreateFishery;

public class CreateFisheryCommandHandler : IRequestHandler<CreateFisheryCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IImageStorageService _imageStorageService;

    public CreateFisheryCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IImageStorageService imageStorageService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _imageStorageService = imageStorageService;
    }

    public async Task<int> Handle(CreateFisheryCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetOrProvisionDomainUserAsync(cancellationToken);
        if (currentUser == null || currentUser.Id == 0)
        {
            throw new UnauthorizedAccessException("Użytkownik musi być zalogowany, aby dodać łowisko.");
        }

        string? imageUrl = null;
        if (request.Image != null && request.Image.Length > 0)
        {
            ImageUploadResult imageResult;
            await using (var memoryStream = new MemoryStream())
            {
                await request.Image.CopyToAsync(memoryStream, cancellationToken);
                memoryStream.Position = 0;
                imageResult = await _imageStorageService.UploadImageAsync(
                    memoryStream,
                    request.Image.FileName,
                    $"fisheries/{currentUser.Id}");
            }

            if (!imageResult.Success || string.IsNullOrEmpty(imageResult.Url))
            {
                throw new ApplicationException($"Nie udało się przesłać zdjęcia dla łowiska: {imageResult.ErrorMessage}");
            }
            imageUrl = imageResult.Url;
        }

        var fishery = new Fishery(
            userId: currentUser.Id,
            name: request.Name,
            location: request.Location,
            imageUrl: imageUrl
        );

        if (request.FishSpeciesIds != null && request.FishSpeciesIds.Any())
        {
            var speciesToAdd = await _context.FishSpecies
                .Where(fs => request.FishSpeciesIds.Contains(fs.Id))
                .ToListAsync(cancellationToken);

            foreach (var species in speciesToAdd)
            {
                fishery.AddSpecies(species);
            }
        }

        _context.Fisheries.Add(fishery);
        await _context.SaveChangesAsync(cancellationToken);

        return fishery.Id;
    }
}