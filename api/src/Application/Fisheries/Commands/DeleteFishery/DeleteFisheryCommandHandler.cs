using Fishio.Application.Common.Exceptions;

namespace Fishio.Application.Fisheries.Commands.DeleteFishery;

public class DeleteFisheryCommandHandler : IRequestHandler<DeleteFisheryCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteFisheryCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteFisheryCommand request, CancellationToken cancellationToken)
    {
        var fishery = await _context.Fisheries
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (fishery == null)
            throw new NotFoundException(nameof(Fishery), request.Id.ToString());


        var currentUserId = _currentUserService.DomainUserId;
        if (fishery.UserId != currentUserId /* && !currentUserService.IsAdmin */)
        {
            throw new ForbiddenAccessException();
        }

        // Opcjonalna walidacja: Czy łowisko jest używane w LogbookEntries?
        // bool isUsedInLogbook = await _context.LogbookEntries.AnyAsync(le => le.FisheryId == request.Id, cancellationToken);
        // if (isUsedInLogbook)
        // {
        //     throw new DeleteFailureException(nameof(Domain.Entities.Fishery), request.Id, "Nie można usunąć łowiska, ponieważ jest powiązane z wpisami w dzienniku.");
        // }

        _context.Fisheries.Remove(fishery);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
