namespace Fishio.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Competition> Competitions { get; }
    DbSet<CompetitionParticipant> CompetitionParticipants { get; }
    DbSet<CompetitionFishCatch> CompetitionFishCatches { get; }
    DbSet<Fishery> Fisheries { get; }
    DbSet<FishSpecies> FishSpecies { get; }
    DbSet<LogbookEntry> LogbookEntries { get; }
    DbSet<ScoringCategoryOption> ScoringCategoryOptions { get; }
    DbSet<SpecialCompetitionCategory> SpecialCompetitionCategories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
