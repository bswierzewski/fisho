using Fishio.Application.About.Models;

namespace Fishio.Application.Common.Interfaces;

public interface IBuildInfoService
{
    BuildInformation GetBuildInformation();
}
