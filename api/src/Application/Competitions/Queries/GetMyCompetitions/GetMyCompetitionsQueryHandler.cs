﻿using Fishio.Application.Competitions.Queries.GetOpenCompetitions;

namespace Fishio.Application.Competitions.Queries.GetMyCompetitions;

public class GetMyCompetitionsQueryHandler : IRequestHandler<GetMyCompetitionsQuery, PaginatedList<CompetitionSummaryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMyCompetitionsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<CompetitionSummaryDto>> Handle(GetMyCompetitionsQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue || currentUserId.Value == 0)
        {
            throw new UnauthorizedAccessException("Użytkownik nie jest zidentyfikowany.");
        }

        IQueryable<Competition> query = _context.Competitions
            .AsNoTracking()
            .Where(c => c.OrganizerId == currentUserId.Value ||
                        c.Participants.Any(p => p.UserId == currentUserId.Value))
            .Include(c => c.Fishery)
            .Include(c => c.Participants) // Potrzebne do filtrowania po roli i liczenia
            .Include(c => c.Categories).ThenInclude(cc => cc.CategoryDefinition);

        switch (request.Filter)
        {
            case MyCompetitionFilter.Upcoming:
                query = query.Where(c => c.Status == CompetitionStatus.AcceptingRegistrations ||
                                         c.Status == CompetitionStatus.Scheduled ||
                                         c.Status == CompetitionStatus.Upcoming ||
                                         c.Status == CompetitionStatus.Ongoing);
                break;
            case MyCompetitionFilter.Finished:
                query = query.Where(c => c.Status == CompetitionStatus.Finished || c.Status == CompetitionStatus.Cancelled);
                break;
            case MyCompetitionFilter.Organized:
                query = query.Where(c => c.OrganizerId == currentUserId.Value);
                break;
            case MyCompetitionFilter.Judged:
                query = query.Where(c => c.Participants.Any(p => p.UserId == currentUserId.Value && p.Role == ParticipantRole.Judge));
                break;
            case MyCompetitionFilter.All:
            default:
                // Bez dodatkowego filtrowania po statusie/roli
                break;
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(c => c.Name.Contains(request.SearchTerm) ||
                                     (c.Fishery != null && c.Fishery.Name.Contains(request.SearchTerm)));
        }

        query = query.OrderByDescending(c => c.Schedule.Start);

        var dtoListQuery = query.Select(c => new CompetitionSummaryDto
        {
            Id = c.Id,
            Name = c.Name,
            StartTime = c.Schedule.Start,
            EndTime = c.Schedule.End,
            Status = c.Status,
            Type = c.Type,
            FisheryName = c.Fishery != null ? c.Fishery.Name : null,
            ImageUrl = c.ImageUrl,
            ParticipantsCount = c.Participants.Count(p => p.Role == ParticipantRole.Competitor || p.Role == ParticipantRole.Guest),
            PrimaryScoringInfo = c.Categories
                                  .Where(cat => cat.IsPrimaryScoring && cat.IsEnabled)
                                  .Select(cat => (cat.CustomNameOverride ?? cat.CategoryDefinition.Name) +
                                                 (cat.CategoryDefinition.Metric != CategoryMetric.NotApplicable ? $" ({GetMetricAbbreviation(cat.CategoryDefinition.Metric)})" : ""))
                                  .FirstOrDefault() ?? "Brak informacji"
        });

        return await PaginatedList<CompetitionSummaryDto>.CreateAsync(dtoListQuery, request.PageNumber, request.PageSize);
    }

    private string GetMetricAbbreviation(CategoryMetric metric)
    {
        return metric switch
        {
            CategoryMetric.LengthCm => "cm",
            CategoryMetric.WeightKg => "kg",
            CategoryMetric.FishCount => "szt.",
            _ => metric.ToString()
        };
    }
}
