using Application.Common.Interfaces.Services;
using Fishio.Application.Common.Exceptions;

namespace Fishio.Application.LogbookEntries.Commands.DeleteLogbookEntry;

public class DeleteLogbookEntryCommandHandler : IRequestHandler<DeleteLogbookEntryCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IImageStorageService _imageStorageService; // Do usunięcia zdjęcia

    public DeleteLogbookEntryCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IImageStorageService imageStorageService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _imageStorageService = imageStorageService;
    }

    public async Task<bool> Handle(DeleteLogbookEntryCommand request, CancellationToken cancellationToken)
    {
        var domainUserId = _currentUserService.UserId;
        if (!domainUserId.HasValue || domainUserId.Value == 0)
        {
            throw new UnauthorizedAccessException("Użytkownik nie jest zidentyfikowany.");
        }

        var logbookEntry = await _context.LogbookEntries
            .FirstOrDefaultAsync(le => le.Id == request.Id, cancellationToken);

        if (logbookEntry == null)
        {
            throw new NotFoundException(nameof(LogbookEntry), request.Id.ToString());
        }

        if (logbookEntry.UserId != domainUserId.Value)
        {
            throw new ForbiddenAccessException();
        }

        // Opcjonalnie: usuń zdjęcie z IImageStorageService, jeśli istnieje
        // TODO: Potrzebujemy przechowywać ImagePublicId w LogbookEntry
        // if (!string.IsNullOrEmpty(logbookEntry.ImagePublicId)) // Zakładając, że mamy ImagePublicId
        // {
        //     await _imageStorageService.DeleteImageAsync(logbookEntry.ImagePublicId);
        // }

        _context.LogbookEntries.Remove(logbookEntry);
        var result = await _context.SaveChangesAsync(cancellationToken);

        return result > 0;
    }
}
