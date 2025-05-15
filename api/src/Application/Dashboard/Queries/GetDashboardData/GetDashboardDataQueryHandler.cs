namespace Fishio.Application.Dashboard.Queries.GetDashboardData;

public class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, DashboardDataDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetDashboardDataQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

public async Task<DashboardDataDto> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.DomainUserId;
        if (userId == null)
        {
            // Powinno być obsługiwane przez autoryzację, ale jako zabezpieczenie
            throw new UnauthorizedAccessException("Użytkownik nie jest zalogowany.");
        }

        var dashboardDto = new DashboardDataDto();

        // 1. User Summary
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user != null)
        {
            dashboardDto.UserSummary.UserName = user.Name;
        }

        // 2. Competition Summary
        var now = DateTimeOffset.UtcNow;

        var userCompetitionsAsParticipant = await _context.CompetitionParticipants
            .AsNoTracking()
            .Where(cp => cp.UserId == userId)
            .Include(cp => cp.Competition)
            .ToListAsync(cancellationToken);

        var userCompetitionsAsOrganizer = await _context.Competitions
            .AsNoTracking()
            .Where(c => c.OrganizerId == userId)
            .ToListAsync(cancellationToken);

        dashboardDto.CompetitionSummary.ParticipatingCount = userCompetitionsAsParticipant
            .Count(cp => cp.Competition.Status != CompetitionStatus.Draft && cp.Competition.Status != CompetitionStatus.Cancelled);

        dashboardDto.CompetitionSummary.OrganizingCount = userCompetitionsAsOrganizer
            .Count(c => c.Status != CompetitionStatus.Draft && c.Status != CompetitionStatus.Cancelled);


        // 3. Upcoming Events (np. 3 najbliższe)
        var upcomingParticipating = userCompetitionsAsParticipant
            .Where(cp => cp.Competition.StartTime > now && (cp.Competition.Status == CompetitionStatus.Scheduled || cp.Competition.Status == CompetitionStatus.AcceptingRegistrations))
            .OrderBy(cp => cp.Competition.StartTime)
            .Select(cp => new UpcomingEventDto
            {
                CompetitionId = cp.CompetitionId,
                CompetitionName = cp.Competition.Name,
                StartTime = cp.Competition.StartTime,
                RoleInCompetition = "Uczestnik",
                Status = cp.Competition.Status
            });

        var upcomingOrganizing = userCompetitionsAsOrganizer
            .Where(c => c.StartTime > now && (c.Status == CompetitionStatus.Scheduled || c.Status == CompetitionStatus.AcceptingRegistrations))
            .OrderBy(c => c.StartTime)
            .Select(c => new UpcomingEventDto
            {
                CompetitionId = c.Id,
                CompetitionName = c.Name,
                StartTime = c.StartTime,
                RoleInCompetition = "Organizator",
                Status = c.Status
            });

        dashboardDto.UpcomingEvents = upcomingParticipating
            .Concat(upcomingOrganizing)
            .OrderBy(e => e.StartTime)
            .Take(3) // Weź 3 najbliższe
            .ToList();

        // 4. Logbook Summary
        var userLogbookEntries = await _context.LogbookEntries
            .AsNoTracking()
            .Where(le => le.UserId == userId)
            .Include(le => le.Fishery) // Opcjonalnie, jeśli chcesz nazwę łowiska
            .OrderByDescending(le => le.CatchTime)
            .ToListAsync(cancellationToken);

        dashboardDto.LogbookSummary.TotalLogbookEntries = userLogbookEntries.Count;
        dashboardDto.LogbookSummary.TotalFishCaughtInLogbook = userLogbookEntries.Count; // Prosta suma, można rozbudować o sumę wag/długości jeśli są zawsze podawane

        dashboardDto.LogbookSummary.RecentEntries = userLogbookEntries
            .Take(3) // Weź 3 ostatnie
            .Select(le => new RecentLogbookEntryDto
            {
                Id = le.Id,
                SpeciesName = le.SpeciesName,
                CatchTime = le.CatchTime,
                LengthCm = le.LengthCm,
                WeightKg = le.WeightKg,
                FisheryName = le.Fishery?.Name
            }).ToList();

        return dashboardDto;
    }
} 