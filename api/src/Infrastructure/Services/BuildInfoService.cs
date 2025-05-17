using Fishio.Application.About.Queries.GetApplicationInfo;
using Microsoft.Extensions.Logging;

namespace Fishio.Infrastructure.Services;

public class BuildInfoService : IBuildInfoService
{
    private readonly ILogger<BuildInfoService> _logger;
    private BuildInformation? _buildInfoCache;

    public BuildInfoService(ILogger<BuildInfoService> logger)
    {
        _logger = logger;
    }

    public BuildInformation GetBuildInformation()
    {
        if (_buildInfoCache != null)
        {
            return _buildInfoCache;
        }

        _logger.LogInformation("Attempting to read build information from environment variables.");

        var gitSha = Environment.GetEnvironmentVariable("APP_VERSION_SHA");
        var buildTimestamp = Environment.GetEnvironmentVariable("APP_BUILD_TIMESTAMP");

        if (string.IsNullOrEmpty(gitSha) && string.IsNullOrEmpty(buildTimestamp))
        {
            _logger.LogWarning("Build information environment variables (APP_VERSION_SHA, APP_BUILD_TIMESTAMP) not found or empty. Returning default build information.");
            _buildInfoCache = new BuildInformation();
        }
        else
        {
            _buildInfoCache = new BuildInformation
            {
                GitSha = !string.IsNullOrEmpty(gitSha) ? gitSha : "N/A",
                BuildTimestamp = !string.IsNullOrEmpty(buildTimestamp) ? buildTimestamp : "N/A",
            };
            _logger.LogInformation("Successfully read build information from environment variables: SHA={GitSha}, Timestamp={BuildTimestamp}",
                _buildInfoCache.GitSha, _buildInfoCache.BuildTimestamp);
        }

        return _buildInfoCache;
    }
}
