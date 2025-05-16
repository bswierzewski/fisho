using Fishio.Domain.Events;

namespace Fishio.Domain.Entities;

public class LogbookEntry : BaseAuditableEntity
{
    public int UserId { get; private set; }
    public virtual User User { get; private set; } = null!;

    public decimal? Length { get; private set; }
    public decimal? Weight { get; private set; }
    public string PhotoUrl { get; private set; } = string.Empty; // Required
    public DateTimeOffset CatchTime { get; private set; }
    public string? Notes { get; private set; }

    public int? FishSpeciesId { get; private set; } 
    public virtual FishSpecies? FishSpecies { get; private set; }

    public int? FisheryId { get; private set; } // Optional link to a fishery
    public virtual Fishery? Fishery { get; private set; }

    // Private constructor for EF Core
    private LogbookEntry() { }

    // Konstruktor do tworzenia nowego wpisu w dzienniku
    public LogbookEntry(
        int userId,
        string? photoUrl,
        DateTimeOffset? catchTime,
        decimal? length = null,
        decimal? weight = null,
        string? notes = null,
        int? fishSpeciesId = null,
        int? fisheryId = null)
    {
        Guard.Against.NegativeOrZero(userId, nameof(userId), "Id użytkownika jest wymagane.");
        Guard.Against.NullOrWhiteSpace(photoUrl, nameof(photoUrl), "URL zdjęcia jest wymagany.");
        Guard.Against.Default(catchTime, nameof(catchTime), "Czas połowu jest wymagany.");

        if (catchTime > DateTimeOffset.UtcNow.AddHours(1))
            throw new ArgumentOutOfRangeException(nameof(catchTime), "Czas połowu nie może być znacznie w przyszłości.");

        if (length.HasValue)
        {
            Guard.Against.NegativeOrZero(length.Value, nameof(length), "Długość musi być wartością dodatnią.");
            Guard.Against.OutOfRange(length.Value, nameof(length), 0.1m, 9999.99m, "Długość poza dopuszczalnym zakresem."); // Przykładowy zakres
        }

        if (weight.HasValue)
        {
            Guard.Against.NegativeOrZero(weight.Value, nameof(weight), "Waga musi być wartością dodatnią.");
            Guard.Against.OutOfRange(weight.Value, nameof(weight), 0.001m, 9999.999m, "Waga poza dopuszczalnym zakresem."); // Przykładowy zakres
        }

        if (fisheryId.HasValue)
            Guard.Against.NegativeOrZero(fisheryId.Value, nameof(fisheryId), "Id łowiska musi być wartością dodatnią.");

        UserId = userId;
        FishSpeciesId = fishSpeciesId;
        CatchTime = catchTime ?? DateTimeOffset.UtcNow;
        PhotoUrl = photoUrl;
        Length = length;
        Weight = weight;
        Notes = notes;
        FisheryId = fisheryId;

        AddDomainEvent(new LogbookEntryCreatedEvent(this));
    }
}