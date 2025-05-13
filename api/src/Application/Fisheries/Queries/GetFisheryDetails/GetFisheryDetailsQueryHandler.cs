namespace Fishio.Application.Fisheries.Queries.GetFisheryDetails;

public class GetFisheryDetailsQueryHandler : IRequestHandler<GetFisheryDetailsQuery, FisheryDetailsDto>
{
    private readonly IApplicationDbContext _context;

    public GetFisheryDetailsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FisheryDetailsDto> Handle(GetFisheryDetailsQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for getting fishery details
        return new FisheryDetailsDto();
    }
} 