namespace Fishio.Application.Users.Queries.SearchUsers;

public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, List<UserDto>>
{
    private readonly IApplicationDbContext _context;

    public SearchUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        var searchTermLower = request.SearchTerm.ToLower();

        var query = _context.Users.AsQueryable();

        query = query.Where(u =>
            (u.Name != null && u.Name.ToLower().Contains(searchTermLower)) ||
            (u.Email != null && u.Email.ToLower().Contains(searchTermLower))
        );

        // Sortowanie, aby wyniki były spójne (np. po nazwie)
        query = query.OrderBy(u => u.Name);

        // Ograniczenie liczby wyników
        query = query.Take(request.MaxResults);

        var usersDto = await query
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email ?? string.Empty,
                ImageUrl = null // DO doimplementowania w przyszłości
            })
            .ToListAsync(cancellationToken);

        return usersDto;
    }
} 