namespace Fishio.Domain.Entities;

public class LogbookEntry : BaseAuditableEntity
{
    public int UserId { get; private set; }
    public virtual User User { get; private set; } = null!;

    public string ImageUrl { get; private set; } = string.Empty;
    public DateTimeOffset CatchTime { get; private set; } = DateTimeOffset.UtcNow;

    public FishLength? Length { get; private set; }
    public FishWeight? Weight { get; private set; }
    public string? Notes { get; private set; }

    public int? FishSpeciesId { get; private set; }
    public virtual FishSpecies? FishSpecies { get; private set; }

    public int? FisheryId { get; private set; }
    public virtual Fishery? Fishery { get; private set; }

    private LogbookEntry() { }

    public LogbookEntry(
        int userId,
        string imageUrl,
        DateTimeOffset? catchTime = null,
        FishLength? length = null,
        FishWeight? weight = null,
        string? notes = null,
        int? fishSpeciesId = null,
        int? fisheryId = null)
    {
        Guard.Against.NegativeOrZero(userId, nameof(userId), "Id użytkownika jest wymagane.");
        Guard.Against.NullOrWhiteSpace(imageUrl, nameof(imageUrl), "URL zdjęcia jest wymagany.");

        if (catchTime.HasValue && catchTime.Value > DateTimeOffset.UtcNow.AddHours(1))
            throw new ArgumentOutOfRangeException(nameof(catchTime), "Czas połowu nie może być znacznie w przyszłości.");
        if (fishSpeciesId.HasValue) Guard.Against.NegativeOrZero(fishSpeciesId.Value, nameof(fishSpeciesId));
        if (fisheryId.HasValue) Guard.Against.NegativeOrZero(fisheryId.Value, nameof(fisheryId));

        UserId = userId;
        ImageUrl = imageUrl;
        CatchTime = catchTime ?? DateTimeOffset.UtcNow;
        Length = length;
        Weight = weight;
        Notes = notes;
        FishSpeciesId = fishSpeciesId;
        FisheryId = fisheryId;

        AddDomainEvent(new LogbookEntryCreatedEvent(this));
    }

    public void UpdateDetails(
        string imageUrl,
        DateTimeOffset? catchTime,
        FishLength? length,
        FishWeight? weight,
        string? notes,
        int? fishSpeciesId,
        int? fisheryId
        /* User modifyingUser - do walidacji uprawnień */)
    {
        // TODO: Dodać walidację, czy modifyingUser (jeśli przekazany) ma uprawnienia (np. jest właścicielem wpisu)
        Guard.Against.NullOrWhiteSpace(imageUrl, nameof(imageUrl), "URL zdjęcia jest wymagany.");
        if (catchTime.HasValue && catchTime.Value > DateTimeOffset.UtcNow.AddHours(1))
            throw new ArgumentOutOfRangeException(nameof(catchTime), "Czas połowu nie może być znacznie w przyszłości.");
        if (fishSpeciesId.HasValue) Guard.Against.NegativeOrZero(fishSpeciesId.Value, nameof(fishSpeciesId));
        if (fisheryId.HasValue) Guard.Against.NegativeOrZero(fisheryId.Value, nameof(fisheryId));

        ImageUrl = imageUrl;
        CatchTime = catchTime ?? CatchTime; // Jeśli null, nie zmieniaj
        Length = length;
        Weight = weight;
        Notes = notes;
        FishSpeciesId = fishSpeciesId;
        FisheryId = fisheryId;

        // AddDomainEvent(new LogbookEntryUpdatedEvent(this));
    }
}
