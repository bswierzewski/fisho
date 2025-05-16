using Fishio.Application.LookupData.Queries.ListFishSpecies;

namespace Fishio.Application.Fisheries.Queries.GetFisheryDetails;

public class GetFisheryDetailsQueryHandler : IRequestHandler<GetFisheryDetailsQuery, FisheryDetailsDto>
{
    private readonly IApplicationDbContext _context;

    public GetFisheryDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
    }

    public async Task<FisheryDetailsDto> Handle(GetFisheryDetailsQuery request, CancellationToken cancellationToken)
    {
        var fishery = await _context.Fisheries
            .AsNoTracking()
            .Include(f => f.User) // Aby pobrać OwnerName
            .Include(f => f.DefinedSpecies) // Aby pobrać listę gatunków
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (fishery == null)
        {
            throw new NotFoundException(nameof(Fishery), request.Id.ToString());
        }

        // Ręczne mapowanie jako przykład (lub użyj AutoMappera)
        return new FisheryDetailsDto
        {
            Id = fishery.Id,
            Name = fishery.Name,
            Location = fishery.Location,
            ImageUrl = fishery.ImageUrl,
            OwnerName = fishery.User?.Name, // User może być null, jeśli UserId jest null
            OwnerId = fishery.UserId,
            FishSpecies = fishery.DefinedSpecies.Select(fs => new FisheryFishSpeciesDto { }).ToList(),
        };
    }
}