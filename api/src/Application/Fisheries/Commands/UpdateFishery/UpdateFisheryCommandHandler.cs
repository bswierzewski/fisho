using Fishio.Application.Common.Exceptions;

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
        var fishery = await _context.Fisheries
                    .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        // var fishery = await _fisheryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (fishery == null)
        {
            throw new NotFoundException(nameof(Fishery), request.Id.ToString());
        }

        var currentUserId = _currentUserService.DomainUserId;
        if (fishery.UserId != currentUserId /* && !currentUserService.IsAdmin */) // Dodaj sprawdzenie roli admina, jeśli jest
        {
            throw new ForbiddenAccessException();
        }

        fishery.UpdateDetails(request.Name, request.Location, request.ImageUrl); // Metoda domenowa

        // _fisheryRepository.UpdateAsync(fishery, cancellationToken); // Jeśli używasz repo
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
} 