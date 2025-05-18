namespace Fishio.Application.Fisheries.Queries.ListFisheries;

public class GetFisheriesListQueryHandler : IRequestHandler<GetFisheriesListQuery, PaginatedList<FisheryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetFisheriesListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FisheryDto>> Handle(GetFisheriesListQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Fishery> query = _context.Fisheries
            .AsNoTracking()
            .Include(f => f.User)       // Do pobrania UserName
            .Include(f => f.FishSpecies); // Do pobrania gatunków ryb

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(f => f.Name.Contains(request.SearchTerm) ||
                                     (f.Location != null && f.Location.Contains(request.SearchTerm)));
        }

        query = query.OrderBy(f => f.Name); // Przykładowe sortowanie

        // Ręczne mapowanie przed paginacją, aby uniknąć problemów z Include po paginacji
        // lub mapowanie po pobraniu spaginowanych encji.
        // Wybierzmy mapowanie po pobraniu spaginowanych encji dla prostoty.

        var paginatedFisheries = await PaginatedList<Fishery>.CreateAsync(query, request.PageNumber, request.PageSize);

        var fisheryDtos = paginatedFisheries.Items.Select(f => new FisheryDto
        {
            Id = f.Id,
            Name = f.Name,
            ImageUrl = f.ImageUrl,
            Location = f.Location,
            UserId = f.UserId,
            UserName = f.User?.Name,
            Created = f.Created,
            FishSpecies = f.FishSpecies.Select(fs => new FishSpeciesSimpleDto { Id = fs.Id, Name = fs.Name }).ToList()
        }).ToList();

        return new PaginatedList<FisheryDto>(fisheryDtos, paginatedFisheries.TotalCount, paginatedFisheries.PageNumber, request.PageSize);
    }
}