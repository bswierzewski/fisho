namespace Fishio.Domain.Entities;

public class Competition : BaseAuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset EndTime { get; private set; }
    public string? Location { get; private set; }
    public string? Rules { get; private set; }
    public CompetitionType Type { get; private set; }
    public CompetitionStatus Status { get; private set; }
    public string? ImageUrl { get; private set; }
    public string ResultsToken { get; private set; } = string.Empty;

    public int OrganizerId { get; private set; }
    public virtual User Organizer { get; private set; } = null!;

    public int FisheryId { get; private set; }
    public virtual Fishery Fishery { get; private set; } = null!;

    public ICollection<CompetitionCategory> Categories { get; private set; } = [];
    public virtual ICollection<CompetitionParticipant> Participants { get; private set; } = [];
    public virtual ICollection<CompetitionFishCatch> FishCatches { get; private set; } = [];

    private Competition() { }
}
