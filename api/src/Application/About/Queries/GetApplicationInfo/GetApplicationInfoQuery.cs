namespace Fishio.Application.About.Queries.GetApplicationInfo;

public record GetApplicationInfoQuery : IRequest<ApplicationInfoDto>;

public record ApplicationInfoDto
{
    public string Name { get; init; } = "Fishio";
    public string Version { get; init; } = "1.0.0";
    public string Description { get; init; } = "Platform for organizing and participating in fishing competitions";
    public List<string> Features { get; init; } = new()
    {
        "Competition Management",
        "Participant Management",
        "Fish Catch Recording",
        "Public Results",
        "Personal Logbook",
        "Fishery Management"
    };
    public Dictionary<string, string> Technologies { get; init; } = new()
    {
        { "Backend", ".NET 9 (Minimal API)" },
        { "Frontend", "Next.js (React, TypeScript, App Router)" },
        { "Database", "PostgreSQL (Production), SQLite (Development)" },
        { "Authentication", "Clerk" }
    };
} 