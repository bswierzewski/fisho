namespace Fishio.Application.About.Queries.GetApplicationInfo;

public class GetApplicationInfoQueryHandler : IRequestHandler<GetApplicationInfoQuery, ApplicationInfoDto>
{
    public Task<ApplicationInfoDto> Handle(GetApplicationInfoQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ApplicationInfoDto());
    }
} 