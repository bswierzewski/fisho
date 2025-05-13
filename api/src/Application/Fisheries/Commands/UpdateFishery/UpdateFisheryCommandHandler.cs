namespace Fishio.Application.Fisheries.Commands.UpdateFishery;

public class UpdateFisheryCommandHandler : IRequestHandler<UpdateFisheryCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateFisheryCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateFisheryCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for updating a fishery
        return Unit.Value;
    }
} 