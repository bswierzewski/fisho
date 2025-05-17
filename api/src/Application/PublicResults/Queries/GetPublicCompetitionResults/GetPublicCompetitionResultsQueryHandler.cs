namespace Fishio.Application.PublicResults.Queries.GetPublicCompetitionResults;

public class GetPublicCompetitionResultsQueryHandler : IRequestHandler<GetPublicCompetitionResultsQuery, PublicCompetitionResultsDto>
{
    private readonly IApplicationDbContext _context;

    public GetPublicCompetitionResultsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PublicCompetitionResultsDto> Handle(GetPublicCompetitionResultsQuery request, CancellationToken cancellationToken)
    {
        var competition = await _context.Competitions
            .AsNoTracking()
            .Include(c => c.Fishery)
            .Include(c => c.Organizer)
            .Include(c => c.Categories)
                .ThenInclude(cc => cc.CategoryDefinition)
            .Include(c => c.Categories)
                .ThenInclude(cc => cc.FishSpecies)
            .Include(c => c.Participants)
                .ThenInclude(p => p.User)
            .Include(c => c.FishCatches)
                .ThenInclude(fc => fc.Participant)
            .FirstOrDefaultAsync(c => c.ResultsToken == request.ResultToken, cancellationToken);

        // Użycie GuardClauses (lub rzucenie NotFoundException bezpośrednio)
        if (competition == null)
        {
            // Guard.Against.Null(competition, nameof(competition), $"Competition with token '{request.ResultToken}' not found.");
            // Powyższe rzuciłoby ArgumentNullException. Lepsze jest bezpośrednie rzucenie NotFoundException.
            throw new NotFoundException(nameof(Competition), request.ResultToken);
        }


        if (competition.Status == CompetitionStatus.Draft || competition.Status == CompetitionStatus.PendingApproval)
        {
            throw new NotFoundException(nameof(request.ResultToken), $"Wyniki dla zawodów o tokenie '{request.ResultToken}' nie są jeszcze publicznie dostępne.");
        }

        var resultDto = new PublicCompetitionResultsDto
        {
            CompetitionId = competition.Id,
            CompetitionName = competition.Name,
            CompetitionStartTime = competition.Schedule.Start,
            CompetitionEndTime = competition.Schedule.End,
            CompetitionStatus = competition.Status,
            CompetitionLocation = competition.Fishery.Location,
            CompetitionImageUrl = competition.ImageUrl,
            OrganizerName = competition.Organizer?.Name
        };

        var allCompetitionCatches = competition.FishCatches.ToList();
        var allCompetitionParticipants = competition.Participants.ToList();

        foreach (var cc in competition.Categories.Where(cat => cat.IsEnabled).OrderBy(cat => cat.SortOrder))
        {
            var categoryResultDto = new PublicCategoryResultDto
            {
                CompetitionCategoryId = cc.Id,
                CategoryDefinitionId = cc.CategoryDefinitionId,
                CategoryName = !string.IsNullOrEmpty(cc.CustomNameOverride) ? cc.CustomNameOverride : cc.CategoryDefinition.Name,
                CategoryDescription = !string.IsNullOrEmpty(cc.CustomDescriptionOverride) ? cc.CustomDescriptionOverride : cc.CategoryDefinition.Description,
                CategoryType = cc.CategoryDefinition.Type,
                CategoryMetric = cc.CategoryDefinition.Metric,
                CategoryCalculationLogic = cc.CategoryDefinition.CalculationLogic,
                SpecificFishSpeciesName = cc.FishSpecies?.Name,
                MaxWinnersToDisplay = cc.MaxWinnersToDisplay,
                IsManuallyAssignedOrNotCalculated = false
            };

            var relevantCatchesForCategory = allCompetitionCatches;
            if (cc.CategoryDefinition.RequiresSpecificFishSpecies && cc.FishSpeciesId.HasValue)
            {
                relevantCatchesForCategory = allCompetitionCatches
                    .Where(f => f.FishSpeciesId == cc.FishSpeciesId)
                    .ToList();
            }
            else if (cc.CategoryDefinition.RequiresSpecificFishSpecies && !cc.FishSpeciesId.HasValue)
                relevantCatchesForCategory = [];

            switch (cc.CategoryDefinition.CalculationLogic)
            {
                case CategoryCalculationLogic.MaxValue:
                    HandleMaxValueCategory(categoryResultDto, relevantCatchesForCategory, allCompetitionParticipants, cc);
                    break;
                case CategoryCalculationLogic.SumValue:
                    HandleSumValueCategory(categoryResultDto, relevantCatchesForCategory, allCompetitionParticipants, cc);
                    break;
                case CategoryCalculationLogic.MinValue: // Nowa logika
                    HandleMinValueCategory(categoryResultDto, relevantCatchesForCategory, allCompetitionParticipants, cc);
                    break;
                case CategoryCalculationLogic.FirstOccurrence:
                    HandleOccurrenceCategory(categoryResultDto, relevantCatchesForCategory, allCompetitionParticipants, cc, true);
                    break;
                case CategoryCalculationLogic.LastOccurrence: // Nowa logika
                    HandleOccurrenceCategory(categoryResultDto, relevantCatchesForCategory, allCompetitionParticipants, cc, false);
                    break;
                case CategoryCalculationLogic.ManualAssignment:
                default:
                    categoryResultDto.IsManuallyAssignedOrNotCalculated = true;
                    categoryResultDto.Note = "Wyniki dla tej kategorii są przypisywane manualnie lub ogłaszane przez organizatora.";
                    if (cc.CategoryDefinition.EntityType == CategoryEntityType.ParticipantProfile &&
                        cc.CategoryDefinition.Name.Contains("Najmłodszy", StringComparison.OrdinalIgnoreCase)) // Prosty przykład dla "Najmłodszy uczestnik"
                    {
                        categoryResultDto.Note = "Zwycięzca zostanie ogłoszony przez organizatora.";
                    }
                    break;
            }
            resultDto.Categories.Add(categoryResultDto);
        }
        return resultDto;
    }

    private string GetParticipantDisplayName(CompetitionParticipant participant)
    {
        return participant.User?.Name ?? participant.GuestName ?? $"Uczestnik #{participant.Id}";
    }

    private PublicFishCatchDto MapToPublicFishCatchDto(CompetitionFishCatch fishCatch)
    {
        return new PublicFishCatchDto
        {
            FishCatchId = fishCatch.Id,
            SpeciesName = fishCatch.FishSpecies?.Name ?? "Nieznany gatunek",
            LengthCm = fishCatch.Length,
            WeightKg = fishCatch.Weight,
            CatchTime = fishCatch.CatchTime,
            PhotoUrl = fishCatch.ImageUrl
        };
    }

    private void AddStandingToCategory(PublicCategoryResultDto categoryDto, CompetitionParticipant participant, decimal? score, string unit, string displayText, IEnumerable<CompetitionFishCatch>? relevantCatches = null)
    {
        // Prosta obsługa rankingu - można rozbudować o obsługę remisów
        int rank = categoryDto.Standings.Count + 1;
        if (categoryDto.Standings.Any() && categoryDto.Standings.Last().ScoreNumeric == score)
        {
            rank = categoryDto.Standings.Last().Rank; // Remis
        }

        categoryDto.Standings.Add(new PublicParticipantStandingDto
        {
            Rank = rank,
            ParticipantId = participant.Id,
            ParticipantName = GetParticipantDisplayName(participant),
            ScoreNumeric = score,
            ScoreUnit = unit,
            ScoreDisplayText = displayText,
            RelevantCatches = relevantCatches?.Select(MapToPublicFishCatchDto).ToList() ?? new List<PublicFishCatchDto>()
        });
    }

    private void HandleMaxValueCategory(PublicCategoryResultDto categoryDto, List<CompetitionFishCatch> catches, List<CompetitionParticipant> participants, CompetitionCategory compCategory)
    {
        if (!catches.Any() && compCategory.CategoryDefinition.EntityType != CategoryEntityType.ParticipantAggregateCatches) return; // Dla SpeciesVariety może nie być 'catches'

        if (compCategory.CategoryDefinition.Metric == CategoryMetric.SpeciesVariety && compCategory.CategoryDefinition.EntityType == CategoryEntityType.ParticipantAggregateCatches)
        {
            var participantScores = participants
                .Select(p => new
                {
                    Participant = p,
                    Score = catches.Where(c => c.ParticipantId == p.Id).Select(c => c.FishSpeciesId).Distinct().Count(),
                    RelevantCatches = catches.Where(c => c.ParticipantId == p.Id).ToList()
                })
                .Where(ps => ps.Score > 0)
                .OrderByDescending(ps => ps.Score)
                .ToList();

            foreach (var ps in participantScores.Take(categoryDto.MaxWinnersToDisplay)) // Stosujemy MaxWinnersToDisplay po sortowaniu
            {
                AddStandingToCategory(categoryDto, ps.Participant, ps.Score, "gatunków", $"{ps.Score} gatunków", ps.RelevantCatches);
            }
            return;
        }


        var scoredCatches = catches.Select(fishCatch =>
        {
            decimal? value = null;
            if (compCategory.CategoryDefinition.Metric == CategoryMetric.LengthCm) value = fishCatch.Length;
            else if (compCategory.CategoryDefinition.Metric == CategoryMetric.WeightKg) value = fishCatch.Weight;
            // Inne metryki dla MaxValue, jeśli będą
            return new { Catch = fishCatch, Value = value, ParticipantId = fishCatch.ParticipantId };
        })
        .Where(sc => sc.Value.HasValue)
        .OrderByDescending(sc => sc.Value)
        .ToList();

        // Dla MaxValue, jeśli EntityType to FishCatch, interesuje nas najlepszy pojedynczy połów.
        // Jeśli ParticipantAggregateCatches, to też najlepszy pojedynczy połów danego uczestnika w tej kategorii.
        // W tym przypadku logika jest podobna.

        var topStandings = new List<PublicParticipantStandingDto>();
        var processedParticipants = new HashSet<int>(); // Aby każdy uczestnik pojawił się raz, jeśli MaxValue dotyczy najlepszego połowu uczestnika

        foreach (var scoredCatch in scoredCatches)
        {
            if (topStandings.Count >= categoryDto.MaxWinnersToDisplay && !processedParticipants.Contains(scoredCatch.ParticipantId))
            {
                // Jeśli już mamy MaxWinnersToDisplay i kolejny jest od nowego uczestnika,
                // a nie jest to remis z ostatnim, to przerywamy (prosta logika bez pełnej obsługi remisów dla wielu miejsc)
                if (topStandings.Any() && scoredCatch.Value < topStandings.Last().ScoreNumeric) break;
            }

            if (processedParticipants.Contains(scoredCatch.ParticipantId) && compCategory.CategoryDefinition.EntityType == CategoryEntityType.ParticipantAggregateCatches)
            {
                // Jeśli już przetworzyliśmy najlepszy połów tego uczestnika dla tej kategorii
                continue;
            }


            var participant = participants.FirstOrDefault(p => p.Id == scoredCatch.ParticipantId);
            if (participant != null)
            {
                string unit = compCategory.CategoryDefinition.Metric == CategoryMetric.LengthCm ? "cm" : "kg";
                AddStandingToCategory(categoryDto, participant, scoredCatch.Value, unit, $"{scoredCatch.Value} {unit}", new List<CompetitionFishCatch> { scoredCatch.Catch });
                processedParticipants.Add(participant.Id);

                if (topStandings.Count >= categoryDto.MaxWinnersToDisplay && categoryDto.Standings.Count(s => s.ScoreNumeric == scoredCatch.Value) == 0)
                {
                    // Jeśli osiągnęliśmy limit i nie ma remisu z ostatnim dodanym, przerwij
                    // Ta logika jest uproszczona dla obsługi remisów na granicy MaxWinnersToDisplay
                }
            }
        }
        // Sortowanie i ograniczanie powinno być bardziej zaawansowane dla pełnej obsługi remisów
        categoryDto.Standings = categoryDto.Standings
                                .OrderByDescending(s => s.ScoreNumeric)
                                .ThenBy(s => GetParticipantDisplayName(participants.First(p => p.Id == s.ParticipantId))) // Stabilne sortowanie dla remisów
                                .Take(categoryDto.MaxWinnersToDisplay)
                                .ToList();
        // Ponowne przypisanie rang po ostatecznym sortowaniu i ograniczeniu
        int currentRank = 0;
        decimal? lastScore = null;
        for (int i = 0; i < categoryDto.Standings.Count; i++)
        {
            if (i == 0 || categoryDto.Standings[i].ScoreNumeric != lastScore)
            {
                currentRank = i + 1;
                lastScore = categoryDto.Standings[i].ScoreNumeric;
            }
            categoryDto.Standings[i].Rank = currentRank;
        }
    }

    private void HandleSumValueCategory(PublicCategoryResultDto categoryDto, List<CompetitionFishCatch> catches, List<CompetitionParticipant> participants, CompetitionCategory compCategory)
    {
        if (!catches.Any()) return;

        var participantScores = participants
            .Select(p =>
            {
                var participantCatches = catches.Where(c => c.ParticipantId == p.Id).ToList();
                decimal totalScore = 0;

                if (compCategory.CategoryDefinition.Metric == CategoryMetric.LengthCm)
                    totalScore = participantCatches.Sum(c => c.Length?.Value ?? 0);
                else if (compCategory.CategoryDefinition.Metric == CategoryMetric.WeightKg)
                    totalScore = participantCatches.Sum(c => c.Weight?.Value ?? 0);
                else if (compCategory.CategoryDefinition.Metric == CategoryMetric.FishCount)
                    totalScore = participantCatches.Count();

                return new
                {
                    Participant = p,
                    Score = totalScore,
                    Catches = participantCatches
                };
            })
            .Where(ps => ps.Score > 0)
            .OrderByDescending(ps => ps.Score)
            .ThenBy(ps => GetParticipantDisplayName(ps.Participant)) // Stabilne sortowanie dla remisów
            .ToList();

        int rank = 0;
        decimal? lastScore = null;
        int displayedCount = 0;

        for (int i = 0; i < participantScores.Count; i++)
        {
            if (displayedCount >= categoryDto.MaxWinnersToDisplay && participantScores[i].Score != lastScore)
            {
                break; // Osiągnięto limit wyświetlania i nie ma remisu z ostatnim wyświetlonym
            }

            if (i == 0 || participantScores[i].Score != lastScore)
            {
                rank = i + 1;
                lastScore = participantScores[i].Score;
            }

            string unit = "";
            if (compCategory.CategoryDefinition.Metric == CategoryMetric.LengthCm) unit = "cm";
            else if (compCategory.CategoryDefinition.Metric == CategoryMetric.WeightKg) unit = "kg";
            else if (compCategory.CategoryDefinition.Metric == CategoryMetric.FishCount) unit = "ryb";

            AddStandingToCategory(categoryDto, participantScores[i].Participant, participantScores[i].Score, unit, $"{participantScores[i].Score} {unit}", participantScores[i].Catches);
            displayedCount++;
            if (categoryDto.Standings.Count > categoryDto.MaxWinnersToDisplay && categoryDto.Standings.Last().ScoreNumeric != categoryDto.Standings[categoryDto.MaxWinnersToDisplay - 1].ScoreNumeric)
            {
                categoryDto.Standings.RemoveAt(categoryDto.Standings.Count - 1); // Usuń nadmiarowe, jeśli nie ma remisu na granicy
            }
        }
        // Ostateczne przycięcie, jeśli dodano więcej z powodu remisów na granicy
        if (categoryDto.Standings.Count > categoryDto.MaxWinnersToDisplay)
        {
            var cutOffScore = categoryDto.Standings[categoryDto.MaxWinnersToDisplay - 1].ScoreNumeric;
            categoryDto.Standings = categoryDto.Standings.Where(s => s.ScoreNumeric >= cutOffScore).ToList();
        }
    }

    private void HandleMinValueCategory(PublicCategoryResultDto categoryDto, List<CompetitionFishCatch> catches, List<CompetitionParticipant> participants, CompetitionCategory compCategory)
    {
        // Przykład: Najmniejsza ryba (długość/waga) lub najszybszy czas do złowienia (choć to bardziej FirstOccurrence)
        // Dla MinValue, logika jest odwrotna do MaxValue.
        if (!catches.Any()) return;

        var scoredCatches = catches.Select(fishCatch =>
        {
            decimal? value = null;
            if (compCategory.CategoryDefinition.Metric == CategoryMetric.LengthCm) value = fishCatch.Length;
            else if (compCategory.CategoryDefinition.Metric == CategoryMetric.WeightKg) value = fishCatch.Weight;
            // Inne metryki dla MinValue
            return new { Catch = fishCatch, Value = value, ParticipantId = fishCatch.ParticipantId };
        })
        .Where(sc => sc.Value.HasValue)
        .OrderBy(sc => sc.Value) // OrderBy zamiast OrderByDescending
        .ThenBy(sc => sc.Catch.CatchTime) // Dodatkowe kryterium dla remisów wartości
        .ToList();

        var topStandings = new List<PublicParticipantStandingDto>();
        var processedParticipants = new HashSet<int>();

        foreach (var scoredCatch in scoredCatches)
        {
            if (topStandings.Count >= categoryDto.MaxWinnersToDisplay && !processedParticipants.Contains(scoredCatch.ParticipantId))
            {
                if (topStandings.Any() && scoredCatch.Value > topStandings.Last().ScoreNumeric) break;
            }
            if (processedParticipants.Contains(scoredCatch.ParticipantId) && compCategory.CategoryDefinition.EntityType == CategoryEntityType.ParticipantAggregateCatches)
            {
                continue;
            }

            var participant = participants.FirstOrDefault(p => p.Id == scoredCatch.ParticipantId);
            if (participant != null)
            {
                string unit = compCategory.CategoryDefinition.Metric == CategoryMetric.LengthCm ? "cm" : "kg";
                AddStandingToCategory(categoryDto, participant, scoredCatch.Value, unit, $"{scoredCatch.Value} {unit}", new List<CompetitionFishCatch> { scoredCatch.Catch });
                processedParticipants.Add(participant.Id);
            }
        }
        categoryDto.Standings = categoryDto.Standings
                                .OrderBy(s => s.ScoreNumeric) // OrderBy
                                .ThenBy(s => GetParticipantDisplayName(participants.First(p => p.Id == s.ParticipantId)))
                                .Take(categoryDto.MaxWinnersToDisplay)
                                .ToList();
        int currentRank = 0;
        decimal? lastScore = null;
        for (int i = 0; i < categoryDto.Standings.Count; i++)
        {
            if (i == 0 || categoryDto.Standings[i].ScoreNumeric != lastScore)
            {
                currentRank = i + 1;
                lastScore = categoryDto.Standings[i].ScoreNumeric;
            }
            categoryDto.Standings[i].Rank = currentRank;
        }
    }

    private void HandleOccurrenceCategory(PublicCategoryResultDto categoryDto, List<CompetitionFishCatch> catches, List<CompetitionParticipant> participants, CompetitionCategory compCategory, bool isFirstOccurrence)
    {
        if (!catches.Any() || compCategory.CategoryDefinition.Metric != CategoryMetric.TimeOfCatch)
        {
            categoryDto.IsManuallyAssignedOrNotCalculated = true;
            categoryDto.Note = "Nie można automatycznie obliczyć dla tej metryki lub brak danych.";
            return;
        }

        var orderedCatches = isFirstOccurrence
            ? catches.OrderBy(c => c.CatchTime).ThenBy(c => c.Id) // Id dla stabilności przy tym samym czasie
            : catches.OrderByDescending(c => c.CatchTime).ThenByDescending(c => c.Id);

        var relevantCatch = orderedCatches.FirstOrDefault();

        if (relevantCatch != null)
        {
            var participant = participants.FirstOrDefault(p => p.Id == relevantCatch.ParticipantId);
            if (participant != null)
            {
                string occurrenceType = isFirstOccurrence ? "Pierwszy" : "Ostatni";
                AddStandingToCategory(categoryDto, participant, null, "czas", $"{occurrenceType} połów o {relevantCatch.CatchTime:HH:mm:ss dd.MM.yyyy}", new List<CompetitionFishCatch> { relevantCatch });
            }
        }
        else
        {
            categoryDto.Note = "Brak zarejestrowanych połowów spełniających kryteria.";
        }
        // Dla Occurrence zwykle jest jeden zwycięzca, ale MaxWinnersToDisplay jest respektowane
        categoryDto.Standings = categoryDto.Standings.Take(categoryDto.MaxWinnersToDisplay).ToList();
        if (categoryDto.Standings.Any()) categoryDto.Standings.First().Rank = 1;
    }
}
