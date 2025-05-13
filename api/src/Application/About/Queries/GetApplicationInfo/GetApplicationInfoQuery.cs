using Fishio.Application.About.Models;

namespace Fishio.Application.About.Queries.GetApplicationInfo;

public record GetApplicationInfoQuery : IRequest<ApplicationInfoDto>;

public record ApplicationInfoDto
{
    public string Name { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public List<string> Features { get; init; } = new();
    public Dictionary<string, string> Technologies { get; init; } = new();
    public BuildInformation BuildInfo { get; init; } = new();
}
