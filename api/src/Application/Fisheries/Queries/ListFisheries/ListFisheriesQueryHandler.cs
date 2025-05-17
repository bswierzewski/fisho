using Fishio.Application.Common.Mappings;

namespace Fishio.Application.Fisheries.Queries.ListFisheries;

public class ListFisheriesQueryHandler : IRequestHandler<ListFisheriesQuery, PaginatedList<FisheryDto>>
{
    private readonly IApplicationDbContext _context;

    public ListFisheriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FisheryDto>> Handle(ListFisheriesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Fisheries.AsNoTracking();

        if (request.UserId.HasValue)
        {
            query = query.Where(f => f.UserId == request.UserId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(f => f.Name.Contains(request.SearchTerm) ||
                                     (f.Location != null && f.Location.Contains(request.SearchTerm)));
        }

        // Ręczne mapowanie i paginacja
        var fisheriesQuery = query
            .Include(f => f.User) // Dla OwnerName
            .Include(f => f.FishSpecies) // Dla FishSpeciesCount
            .OrderBy(f => f.Name)
            .Select(f => new FisheryDto
            {
                Id = f.Id,
                Name = f.Name,
                Location = f.Location,
                ImageUrl = f.ImageUrl,
                FishSpeciesCount = f.FishSpecies.Count,
            });

        return await fisheriesQuery.ToPaginatedListAsync(request.Page ?? 1, request.PageSize ?? 10);
    }
}