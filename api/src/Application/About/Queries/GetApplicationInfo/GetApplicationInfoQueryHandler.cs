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
            Description = "Platforma do organizowania i udziału w zawodach wędkarskich",
            Features = new()
            {
                "Zarządzanie zawodami",
                "Zarządzanie uczestnikami",
                "Rejestracja złowionych ryb",
                "Wyniki publiczne",
                "Osobisty dziennik połowów",
                "Zarządzanie łowiskami"
            },
            BuildInfo = buildInfo
        };

        return Task.FromResult(dto); // Zwracamy opakowane w Task
    }
}
