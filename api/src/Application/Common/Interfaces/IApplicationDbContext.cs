namespace Fishio.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Competition> Competitions { get; }
    DbSet<CompetitionParticipant> CompetitionParticipants { get; }
    DbSet<CompetitionFishCatch> CompetitionFishCatches { get; }
    DbSet<LogbookEntry> LogbookEntries { get; }
    DbSet<Fishery> Fisheries { get; }
    DbSet<FishSpecies> FishSpecies { get; }
    DbSet<CategoryDefinition> CategoryDefinitions { get; }
    DbSet<CompetitionCategory> CompetitionCategories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
