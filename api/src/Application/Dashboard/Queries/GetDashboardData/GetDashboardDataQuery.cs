namespace Fishio.Application.Dashboard.Queries.GetDashboardData;

public class GetDashboardDataQuery : IRequest<DashboardDto>
{
    private const int DefaultPageSize = 5;
    private const int MaxPageSize = 20; // Maksymalna liczba elementów, jaką można zażądać

    private int _maxRecentItems = DefaultPageSize;
    public int? MaxRecentItems
    {
        get => _maxRecentItems;
        set => _maxRecentItems = (value.HasValue && value.Value > 0 && value.Value <= MaxPageSize) ? value.Value : DefaultPageSize;
    }

    private int _maxFeaturedItems = DefaultPageSize;
    public int? MaxFeaturedItems
    {
        get => _maxFeaturedItems;
        set => _maxFeaturedItems = (value.HasValue && value.Value > 0 && value.Value <= MaxPageSize) ? value.Value : DefaultPageSize;
    }
}

// Używamy record dla DTOs, ponieważ są to proste nośniki danych
public record DashboardCompetitionSummaryDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTimeOffset StartTime { get; init; }
    public CompetitionStatus Status { get; init; }
    public string? FisheryName { get; init; } // Nazwa łowiska
    public bool IsOrganizer { get; init; }
    public bool IsJudge { get; init; }
    public bool IsParticipant { get; init; }
}

public record DashboardLogbookSummaryDto
{
    public int Id { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public DateTimeOffset CatchTime { get; init; }
    public string? FishSpeciesName { get; init; }
    public decimal? LengthInCm { get; init; }
    public decimal? WeightInKg { get; init; }
}

public record DashboardFisherySummaryDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Location { get; init; }
    public string? ImageUrl { get; init; }
    public int FishSpeciesCount { get; init; }
}

public record DashboardDto
{
    public string UserName { get; init; } = string.Empty;
    public List<DashboardCompetitionSummaryDto> MyUpcomingCompetitions { get; init; } = new(); // Zawody, w których biorę udział lub organizuję/sędziuję, które się jeszcze nie odbyły lub trwają
    public List<DashboardCompetitionSummaryDto> MyRecentCompetitions { get; init; } = new(); // Ostatnie zakończone zawody, w których brałem udział
    public List<DashboardLogbookSummaryDto> RecentLogbookEntries { get; init; } = new(); // Ostatnie 3-5 wpisów
    public List<DashboardCompetitionSummaryDto> OpenCompetitions { get; init; } = new(); // Kilka otwartych zawodów do odkrycia
    public List<DashboardFisherySummaryDto> FeaturedFisheries { get; init; } = new(); // Kilka łowisk do odkrycia
}