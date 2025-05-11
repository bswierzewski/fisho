namespace Domain.Entities;

// Plik: LogbookEntry.cs
public class LogbookEntry : BaseAuditableEntity
{
    public int UserId { get; set; } // Powinno być mapowane na CreatedBy
    public string SpeciesName { get; set; } = string.Empty;
    public decimal? LengthCm { get; set; }
    public decimal? WeightKg { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public DateTimeOffset CatchTime { get; set; } // Zmieniono na DateTimeOffset
    public int? FisheryId { get; set; }
    public string? Notes { get; set; }

    // Właściwości nawigacyjne
    public virtual User User { get; set; } = null!;
    public virtual Fishery? Fishery { get; set; }
}