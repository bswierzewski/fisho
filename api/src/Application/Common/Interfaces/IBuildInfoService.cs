using Fishio.Application.About.Queries.GetApplicationInfo;

namespace Fishio.Application.Common.Interfaces;

public interface IBuildInfoService
{
    BuildInformation GetBuildInformation();
}