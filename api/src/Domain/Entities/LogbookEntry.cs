namespace Fishio.Domain.Entities;

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

    // Konstruktor do tworzenia nowego wpisu w dzienniku
    public LogbookEntry(
        int userId,
        string speciesName,
        DateTimeOffset catchTime,
        string photoUrl,
        decimal? lengthCm = null,
        decimal? weightKg = null,
        string? notes = null,
        int? fisheryId = null)
    {
        Guard.Against.NegativeOrZero(userId, nameof(userId), "Id użytkownika jest wymagane.");
        Guard.Against.NullOrWhiteSpace(speciesName, nameof(speciesName), "Nazwa gatunku jest wymagana.");
        Guard.Against.LengthOutOfRange(speciesName, 1, 255, "Nazwa gatunku musi mieć od 1 do 255 znaków.");
        Guard.Against.NullOrWhiteSpace(photoUrl, nameof(photoUrl), "URL zdjęcia jest wymagany.");
        Guard.Against.Default(catchTime, nameof(catchTime), "Czas połowu jest wymagany.");

        if (catchTime > DateTimeOffset.UtcNow.AddHours(1))
            throw new ArgumentOutOfRangeException(nameof(catchTime), "Czas połowu nie może być znacznie w przyszłości.");

        if (lengthCm.HasValue)
        {
            Guard.Against.NegativeOrZero(lengthCm.Value, nameof(lengthCm), "Długość musi być wartością dodatnią.");
            Guard.Against.OutOfRange(lengthCm.Value, nameof(lengthCm), 0.1m, 9999.99m, "Długość poza dopuszczalnym zakresem."); // Przykładowy zakres
        }

        if (weightKg.HasValue)
        {
            Guard.Against.NegativeOrZero(weightKg.Value, nameof(weightKg), "Waga musi być wartością dodatnią.");
            Guard.Against.OutOfRange(weightKg.Value, nameof(weightKg), 0.001m, 9999.999m, "Waga poza dopuszczalnym zakresem."); // Przykładowy zakres
        }

        if (fisheryId.HasValue)
            Guard.Against.NegativeOrZero(fisheryId.Value, nameof(fisheryId), "Id łowiska musi być wartością dodatnią.");

        UserId = userId;
        SpeciesName = speciesName;
        CatchTime = catchTime;
        PhotoUrl = photoUrl;
        LengthCm = lengthCm;
        WeightKg = weightKg;
        Notes = notes;
        FisheryId = fisheryId;

        AddDomainEvent(new LogbookEntryCreatedEvent(this));
    }
}