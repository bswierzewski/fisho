namespace Fishio.Domain.Entities
{
    public class SpecialCompetitionCategory : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        public int CompetitionId { get; private set; }
        public virtual Competition Competition { get; private set; } = null!;

        // Private constructor for EF Core
        private SpecialCompetitionCategory() { }

        public SpecialCompetitionCategory(Competition competition, string name, string? description)
        {
            Competition = competition;
            CompetitionId = competition.Id;
            Name = name;
            Description = description;
        }
    }
}
