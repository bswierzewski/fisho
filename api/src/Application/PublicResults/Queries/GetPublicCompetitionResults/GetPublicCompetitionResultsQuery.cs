namespace Fishio.Application.PublicResults.Queries.GetPublicCompetitionResults;

public record GetPublicCompetitionResultsQuery : IRequest<PublicCompetitionResultsDto>
{
    public string ResultToken { get; init; } = string.Empty;
}

public class GetPublicCompetitionResultsQueryValidator : AbstractValidator<GetPublicCompetitionResultsQuery>
{
    public GetPublicCompetitionResultsQueryValidator()
    {
        RuleFor(x => x.ResultToken)
                    .NotEmpty().WithMessage("Token wyników jest wymagany.")
                    .Length(10, 64).WithMessage("Token wyników musi mieć od 10 do 64 znaków.");
    }
}

public class PublicCompetitionResultsDto
{
    public int CompetitionId { get; set; }
    public required string CompetitionName { get; set; }
    public DateTimeOffset CompetitionStartTime { get; set; }
    public DateTimeOffset CompetitionEndTime { get; set; }
    public CompetitionStatus CompetitionStatus { get; set; }
    public string? CompetitionLocationText { get; set; }
    public string? CompetitionImageUrl { get; set; }
    public string? OrganizerName { get; set; } // Optional: if you want to display it
    public List<PublicCategoryResultDto> Categories { get; set; } = new();
}

public class PublicCategoryResultDto
{
    public int CompetitionCategoryId { get; set; } // Id from CompetitionCategory
    public int CategoryDefinitionId { get; set; }
    public required string CategoryName { get; set; }
    public string? CategoryDescription { get; set; }
    public CategoryType CategoryType { get; set; }
    public CategoryMetric CategoryMetric { get; set; }
    public CategoryCalculationLogic CategoryCalculationLogic { get; set; }
    public string? SpecificFishSpeciesName { get; set; }
    public int MaxWinnersToDisplay { get; set; }
    public bool IsManuallyAssignedOrNotCalculated { get; set; } // True if manual or logic not yet implemented for auto-calc
    public string? Note { get; set; } // e.g., "Winners announced by organizer"
    public List<PublicParticipantStandingDto> Standings { get; set; } = new();
}

public class PublicParticipantStandingDto
{
    public int Rank { get; set; }
    public int ParticipantId { get; set; } // CompetitionParticipant.Id
    public required string ParticipantName { get; set; }
    public decimal? ScoreNumeric { get; set; }
    public string? ScoreUnit { get; set; }
    public required string ScoreDisplayText { get; set; }
    public List<PublicFishCatchDto> RelevantCatches { get; set; } = new();
}

public class PublicFishCatchDto
{
    public int FishCatchId { get; set; }
    public required string SpeciesName { get; set; }
    public decimal? LengthCm { get; set; }
    public decimal? WeightKg { get; set; }
    public DateTimeOffset CatchTime { get; set; }
    public string? PhotoUrl { get; set; }
}