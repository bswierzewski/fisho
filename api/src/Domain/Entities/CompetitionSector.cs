//namespace Fishio.Domain.Entities;

//public class CompetitionSector : BaseAuditableEntity
//{
//    public int CompetitionId { get; private set; }
//    public virtual Competition Competition { get; private set; } = null!;

//    public string Name { get; private set; } = string.Empty;
//    public string? Description { get; private set; }

//    public virtual ICollection<CompetitionStand> Stands { get; private set; } = [];

//    private CompetitionSector() { }

//    public CompetitionSector(
//        int competitionId,
//        string name,
//        string? description = null
//    )
//    {
//        CompetitionId = competitionId;
//        Name = name;
//        Description = description;
//        Stands = new HashSet<CompetitionStand>();
//    }
//}
