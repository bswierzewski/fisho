namespace Fishio.Application.Fisheries.Commands.CreateFishery;

public class CreateFisheryCommandHandler : IRequestHandler<CreateFisheryCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateFisheryCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateFisheryCommand request, CancellationToken cancellationToken)
    {
        var creatorUserId = _currentUserService.DomainUserId;

        Guard.Against.Null(creatorUserId, nameof(creatorUserId), "Użytkownik musi być zalogowany, aby utworzyć łowisko.");

        var fishery = new Fishery(
            request.Name,
            creatorUserId.Value, // Przekazujemy Id zalogowanego użytkownika
            request.Location,
            request.ImageUrl
        );

        if (request.FishSpeciesIds != null && request.FishSpeciesIds.Any())
        {
            var speciesToAdd = await _context.FishSpecies
                .Where(fs => request.FishSpeciesIds.Contains(fs.Id))
                .ToListAsync(cancellationToken);

            foreach (var species in speciesToAdd)
            {
                fishery.AddSpecies(species); // Metoda domenowa
            }
        }

        _context.Fisheries.Add(fishery);

        await _context.SaveChangesAsync(cancellationToken);

        return fishery.Id;
    }
} 