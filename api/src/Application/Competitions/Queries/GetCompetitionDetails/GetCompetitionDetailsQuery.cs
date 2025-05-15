namespace Fishio.Application.Competitions.Queries.GetCompetitionDetails;

public record GetCompetitionDetailsQuery : IRequest<CompetitionDetailsDto>
{
    public int CompetitionId { get; init; }
}

public class GetCompetitionDetailsQueryValidator : AbstractValidator<GetCompetitionDetailsQuery>
{
    public GetCompetitionDetailsQueryValidator()
    {
        RuleFor(x => x.CompetitionId)
            .NotEmpty().WithMessage("Competition ID is required");
    }
}

public record CompetitionDetailsDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string OrganizerId { get; init; } = string.Empty;
    public string OrganizerName { get; init; } = string.Empty;
    public int ParticipantsCount { get; init; }
    public int JudgesCount { get; init; }
    public int CatchesCount { get; init; }
    public bool IsCurrentUserParticipant { get; init; }
    public bool IsCurrentUserJudge { get; init; }
    public bool IsCurrentUserOrganizer { get; init; }
    public List<CompetitionCategoryDetailsDto> ConfiguredCategories { get; set; } = new();
    public List<CompetitionFishSpeciesDto> FishSpecies { get; init; } = new();
    public List<CompetitionParticipantDto> Participants { get; init; } = new();
    public List<CompetitionCatchDto> RecentCatches { get; init; } = new();
}

public record CompetitionFishSpeciesDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int CatchesCount { get; init; }
}

public record CompetitionParticipantDto
{
    public string UserId { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public bool IsGuest { get; init; }
    public DateTime JoinedAt { get; init; }
    public int CatchesCount { get; init; }
}

public record CompetitionCatchDto
{
    public int Id { get; init; }
    public string ParticipantId { get; init; } = string.Empty;
    public string ParticipantName { get; init; } = string.Empty;
    public int FishSpeciesId { get; init; }
    public string FishSpeciesName { get; init; } = string.Empty;
    public decimal Length { get; init; }
    public decimal? Weight { get; init; }
    public string? Notes { get; init; }
    public DateTime CaughtAt { get; init; }
    public string RecordedByUserId { get; init; } = string.Empty;
    public string RecordedByUserName { get; init; } = string.Empty;
}

public class CompetitionCategoryDetailsDto
{
    public int Id { get; set; } // Id encji CompetitionCategory
    public int CategoryDefinitionId { get; set; }
    public required string CategoryDefinitionName { get; set; } // Nazwa z CategoryDefinition
    public string? CustomNameOverride { get; set; }
    public string DisplayName => !string.IsNullOrEmpty(CustomNameOverride) ? CustomNameOverride : CategoryDefinitionName;

    public string? CategoryDefinitionDescription { get; set; }
    public string? CustomDescriptionOverride { get; set; }
    public string? DisplayDescription => !string.IsNullOrEmpty(CustomDescriptionOverride) ? CustomDescriptionOverride : CategoryDefinitionDescription;

    public CategoryType CategoryType { get; set; }
    public CategoryMetric CategoryMetric { get; set; }
    public CategoryCalculationLogic CategoryCalculationLogic { get; set; }
    public bool RequiresSpecificFishSpecies { get; set; }

    public int? SpecificFishSpeciesId { get; set; }
    public string? SpecificFishSpeciesName { get; set; }

    public int SortOrder { get; set; }
    public bool IsPrimaryScoring { get; set; }
    public int MaxWinnersToDisplay { get; set; }
    public bool IsEnabled { get; set; }
}