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
        var creatorUserId = _currentUserService.UserId;

        Guard.Against.Null(creatorUserId, nameof(creatorUserId), "Użytkownik musi być zalogowany, aby utworzyć łowisko.");
        Guard.Against.NullOrWhiteSpace(request.Name, nameof(request.Name), "Nazwa łowiska jest wymagana.");

        var fishery = new Fishery(
            creatorUserId.Value,
            request.Name,
            request.ImageUrl,
            request.Location
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