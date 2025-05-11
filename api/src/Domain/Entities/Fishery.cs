namespace Domain.Entities;

public class Fishery : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? LocationText { get; set; }
    public string? ImageUrl { get; set; }

    // Właściwości nawigacyjne
    public virtual ICollection<FishSpecies> DefinedSpecies { get; set; } = new List<FishSpecies>();
    public virtual ICollection<LogbookEntry> LogbookEntries { get; set; } = new List<LogbookEntry>();
}
