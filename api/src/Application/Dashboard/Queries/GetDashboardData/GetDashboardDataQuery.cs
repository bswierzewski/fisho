namespace Fishio.Application.Dashboard.Queries.GetDashboardData;

public record GetDashboardDataQuery : IRequest<DashboardDataDto>;

public class DashboardDataDto
{
    public UserSummaryDto UserSummary { get; set; } = new();
    public CompetitionSummaryDto CompetitionSummary { get; set; } = new();
    public LogbookSummaryDto LogbookSummary { get; set; } = new();
    public List<UpcomingEventDto> UpcomingEvents { get; set; } = new();
}

public class UserSummaryDto
{
    public string UserName { get; set; } = string.Empty;
    // Można dodać więcej informacji o użytkowniku, jeśli potrzebne
}

public class CompetitionSummaryDto
{
    public int ParticipatingCount { get; set; }
    public int OrganizingCount { get; set; }
}

public class LogbookSummaryDto
{
    public int TotalLogbookEntries { get; set; }
    public int TotalFishCaughtInLogbook { get; set; } // Przykładowa statystyka
    public List<RecentLogbookEntryDto> RecentEntries { get; set; } = new();
}

public class RecentLogbookEntryDto
{
    public int Id { get; set; }
    public required string SpeciesName { get; set; }
    public DateTimeOffset CatchTime { get; set; }
    public decimal? LengthCm { get; set; }
    public decimal? WeightKg { get; set; }
    public string? FisheryName { get; set; } // Opcjonalnie
}

public class UpcomingEventDto // Może to być zarówno zawody, w których uczestniczy, jak i te, które organizuje
{
    public int CompetitionId { get; set; }
    public required string CompetitionName { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public string RoleInCompetition { get; set; } = string.Empty; // Np. "Organizator", "Uczestnik"
    public Domain.Enums.CompetitionStatus Status { get; set; }
}