namespace Domain.Entities;

// Plik: Competition.cs
public class Competition : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public string? LocationText { get; set; }
    public string? RulesText { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int OrganizerId { get; set; }
    public string ResultsToken { get; set; } = string.Empty;
    public int? MainScoringCategoryId { get; set; }

    // Właściwości nawigacyjne
    public virtual ScoringCategoryOption? MainScoringCategory { get; set; }
    public virtual ICollection<SpecialCategoryOption> SelectedSpecialCategories { get; set; } = new List<SpecialCategoryOption>();
    public virtual ICollection<CompetitionParticipant> Participants { get; set; } = new List<CompetitionParticipant>();
    public virtual ICollection<CompetitionFishCatch> FishCatches { get; set; } = new List<CompetitionFishCatch>();
}
