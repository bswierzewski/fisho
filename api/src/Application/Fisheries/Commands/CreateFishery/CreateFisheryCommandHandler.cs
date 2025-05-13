namespace Fishio.Application.Fisheries.Commands.CreateFishery;

public class CreateFisheryCommandHandler : IRequestHandler<CreateFisheryCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateFisheryCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateFisheryCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for creating a fishery
        return 0;
    }
} 