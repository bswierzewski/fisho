namespace Domain.Entities;

public class FishSpecies : BaseEntity
{
    public string Name { get; set; }
    public virtual ICollection<Fishery> Fisheries { get; set; } = new List<Fishery>();
}
