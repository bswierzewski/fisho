using Fishio.Application.Common.Interfaces;

namespace Fishio.Application.About.Queries.GetApplicationInfo;

public class GetApplicationInfoQueryHandler : IRequestHandler<GetApplicationInfoQuery, ApplicationInfoDto>
{
    private readonly IBuildInfoService _buildInfoService;

    public GetApplicationInfoQueryHandler(IBuildInfoService buildInfoService)
    {
        _buildInfoService = buildInfoService;
    }

    public Task<ApplicationInfoDto> Handle(GetApplicationInfoQuery request, CancellationToken cancellationToken)
    {
        var buildInfo = _buildInfoService.GetBuildInformation();

        var dto = new ApplicationInfoDto
        {
            Name = "Fishio",
            Version = "1.0.0",
            Description = "Platform for organizing and participating in fishing competitions",
            Features = new()
            {
                "Competition Management",
                "Participant Management",
                "Fish Catch Recording",
                "Public Results",
                "Personal Logbook",
                "Fishery Management"
            },
            Technologies = new()
            {
                { "Backend", ".NET 9 (Minimal API)" },
                { "Frontend", "Next.js (React, TypeScript, App Router)" },
                { "Database", "PostgreSQL (Production), SQLite (Development)" },
                { "Authentication", "Clerk" }
            },
            BuildInfo = buildInfo
        };

        return Task.FromResult(dto); // Zwracamy opakowane w Task
    }
}