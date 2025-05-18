namespace Fishio.Application.PublicResults.Queries.GetPublicCompetitionResults;

public class GetPublicCompetitionResultsQuery : IRequest<PublicCompetitionResultsDto?> // Może zwrócić null, jeśli token nieprawidłowy
{
    public string ResultsToken { get; set; } = string.Empty;

    public GetPublicCompetitionResultsQuery(string resultsToken)
    {
        ResultsToken = resultsToken;
    }
}

public class GetPublicCompetitionResultsQueryValidator : AbstractValidator<GetPublicCompetitionResultsQuery>
{
    public GetPublicCompetitionResultsQueryValidator()
    {
        RuleFor(x => x.ResultsToken)
                    .NotEmpty().WithMessage("Token wyników jest wymagany.")
                    .Length(10, 64).WithMessage("Token wyników musi mieć od 10 do 64 znaków.");
    }
}

// --- DTOs składowe ---
public record PublicResultParticipantDto
{
    public int ParticipantId { get; init; } // Może to być ID CompetitionParticipant
    public string Name { get; init; } = string.Empty; // Imię/Nazwisko/Ksywka
    public decimal TotalScore { get; init; } // Główny wynik (np. suma wag, suma długości)
    public int FishCount { get; init; }
    // Można dodać inne zagregowane dane, jeśli potrzebne
}

public record PublicResultCategoryWinnerDto
{
    public int ParticipantId { get; init; }
    public string ParticipantName { get; init; } = string.Empty;
    public string? FishSpeciesName { get; init; } // Jeśli kategoria dotyczy konkretnej ryby
    public decimal? Value { get; init; } // Wartość, która dała zwycięstwo (np. długość, waga)
    public string? Unit { get; init; } // Jednostka dla wartości (cm, kg)
}

public record PublicResultSpecialCategoryDto
{
    public string CategoryName { get; init; } = string.Empty;
    public string? CategoryDescription { get; init; }
    public List<PublicResultCategoryWinnerDto> Winners { get; init; } = new();
}

// --- Główne DTO ---
public record PublicCompetitionResultsDto
{
    // Informacje o zawodach
    public int CompetitionId { get; init; }
    public string CompetitionName { get; init; } = string.Empty;
    public DateTimeOffset StartTime { get; init; }
    public DateTimeOffset EndTime { get; init; }
    public string? FisheryName { get; init; }
    public string? CompetitionImageUrl { get; init; }
    public CompetitionStatus Status { get; init; }
    public string? Rules { get; init; }

    // Informacje o głównej kategorii punktacji
    public string? PrimaryScoringCategoryName { get; init; }
    public CategoryMetric? PrimaryScoringMetric { get; init; } // Np. Długość, Waga

    // Dane zależne od statusu
    public string? UpcomingMessage { get; init; } // Dla statusu Upcoming

    public List<PublicResultParticipantDto> MainRanking { get; init; } = new(); // Dla Ongoing i Finished
    public string? LiveRankingPlaceholderMessage { get; init; } // Dla Ongoing

    public List<PublicResultSpecialCategoryDto> SpecialCategoriesResults { get; init; } = new(); // Dla Finished
    public string? FinishedChartsPlaceholderMessage { get; init; } // Dla Finished
}