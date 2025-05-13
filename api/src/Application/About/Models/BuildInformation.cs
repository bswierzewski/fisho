namespace Fishio.Application.About.Models;

public record BuildInformation
{
    public string GitSha { get; init; } = "N/A";
    public string BuildTimestamp { get; init; } = "N/A";
}
