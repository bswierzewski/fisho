namespace Fishio.Application.Dashboard.Queries.GetDashboardData;

public record GetDashboardDataQuery : IRequest<DashboardDto>;

public record DashboardDto
{
    public int TotalCompetitionsCount { get; init; }
    public int ActiveCompetitionsCount { get; init; }
    public int UpcomingCompetitionsCount { get; init; }
    public int MyCompetitionsCount { get; init; }
    public int MyJudgedCompetitionsCount { get; init; }
    public int MyTotalCatchesCount { get; init; }
    public List<RecentCompetitionDto> RecentCompetitions { get; init; } = new();
    public List<RecentCatchDto> RecentCatches { get; init; } = new();
}

public record RecentCompetitionDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int ParticipantsCount { get; init; }
}

public record RecentCatchDto
{
    public Guid Id { get; init; }
    public string CompetitionName { get; init; } = string.Empty;
    public string FishSpeciesName { get; init; } = string.Empty;
    public decimal Length { get; init; }
    public decimal? Weight { get; init; }
    public DateTime CaughtAt { get; init; }
} 