using System.Reflection;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Competition> Competitions => Set<Competition>();
    public DbSet<CompetitionParticipant> CompetitionParticipants => Set<CompetitionParticipant>();
    public DbSet<CompetitionFishCatch> CompetitionFishCatches => Set<CompetitionFishCatch>();
    public DbSet<Fishery> Fisheries => Set<Fishery>();
    public DbSet<FishSpecies> FishSpecies => Set<FishSpecies>();
    public DbSet<LogbookEntry> LogbookEntries => Set<LogbookEntry>();
    public DbSet<ScoringCategoryOption> ScoringCategoryOptions => Set<ScoringCategoryOption>();
    public DbSet<SpecialCompetitionCategory> SpecialCompetitionCategories => Set<SpecialCompetitionCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
