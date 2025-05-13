namespace Fishio.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<LogbookEntry> LogbookEntries { get; }
    DbSet<Fishery> Fisheries { get; }
    DbSet<FishSpecies> FishSpecies { get; }
    DbSet<User> Users { get; }
    DbSet<Competition> Competitions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
