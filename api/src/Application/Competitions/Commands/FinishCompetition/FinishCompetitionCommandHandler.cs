﻿using Fishio.Application.Common.Exceptions;

namespace Fishio.Application.Competitions.Commands.FinishCompetition;

public class FinishCompetitionCommandHandler : IRequestHandler<FinishCompetitionCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public FinishCompetitionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(FinishCompetitionCommand request, CancellationToken cancellationToken)
    {
        var competition = await _context.Competitions
            .FirstOrDefaultAsync(c => c.Id == request.CompetitionId, cancellationToken);

        if (competition == null)
        {
            throw new NotFoundException(nameof(Competition), request.CompetitionId.ToString());
        }

        var organizerId = _currentUserService.UserId;
        if (competition.OrganizerId != organizerId)
        {
            throw new ForbiddenAccessException("Tylko organizator może zakończyć zawody.");
        }

        try
        {
            competition.FinishCompetition(); // Wywołanie metody domenowej
        }
        catch (InvalidOperationException ex)
        {
            throw new ApplicationException($"Nie można zakończyć zawodów: {ex.Message}", ex);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
