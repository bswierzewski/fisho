namespace Domain.Entities;

// Plik: LogbookEntry.cs
public class LogbookEntry : BaseAuditableEntity
{
    public int UserId { get; private set; }
    public virtual User User { get; private set; } = null!;

    public string SpeciesName { get; private set; } = string.Empty; // Could link to FishSpecies if desired
    public decimal? LengthCm { get; private set; }
    public decimal? WeightKg { get; private set; }
    public string PhotoUrl { get; private set; } = string.Empty; // Required
    public DateTimeOffset CatchTime { get; private set; }
    public string? Notes { get; private set; }

    public int? FisheryId { get; private set; } // Optional link to a fishery
    public virtual Fishery? Fishery { get; private set; }

    // Private constructor for EF Core
    private LogbookEntry() { }

    public LogbookEntry(
        User user,
        string speciesName,
        string photoUrl,
        DateTimeOffset catchTime)
    {
        User = user;
        UserId = user.Id;
        SpeciesName = speciesName;
        PhotoUrl = photoUrl;
        CatchTime = catchTime;
    }
}