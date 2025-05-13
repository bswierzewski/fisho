namespace Fishio.Application.Fisheries.Queries.ListFisheries;

public class ListFisheriesQueryHandler : IRequestHandler<ListFisheriesQuery, List<FisheryDto>>
{
    private readonly IApplicationDbContext _context;

    public ListFisheriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<FisheryDto>> Handle(ListFisheriesQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for listing fisheries
        return new List<FisheryDto>();
    }
} 