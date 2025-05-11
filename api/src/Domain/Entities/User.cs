using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

// Plik: User.cs (Podstawowe dane, reszta w Clerk)
public class User : BaseAuditableEntity
{
    public string ClerkUserId { get; set; } = string.Empty; // Wymagane, unikalne (skonfigurujemy w DbContext)
    public string Name { get; set; } = string.Empty; // Wymagane
    public string? Email { get; set; } // Opcjonalne, unikalne (skonfigurujemy w DbContext)


    // Właściwości nawigacyjne
    public virtual ICollection<Competition> OrganizedCompetitions { get; set; } = new List<Competition>();
    public virtual ICollection<CompetitionParticipant> CompetitionParticipations { get; set; } = new List<CompetitionParticipant>();
    public virtual ICollection<CompetitionFishCatch> JudgedCatches { get; set; } = new List<CompetitionFishCatch>();
    public virtual ICollection<LogbookEntry> LogbookEntries { get; set; } = new List<LogbookEntry>();
    public virtual ICollection<Fishery> CreatedFisheries { get; set; } = new List<Fishery>();
}
