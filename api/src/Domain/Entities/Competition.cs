﻿namespace Fishio.Domain.Entities;

public class Competition : BaseAuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public DateTimeRange Schedule { get; private set; } = null!;
    public string? Rules { get; private set; }
    public CompetitionType Type { get; private set; }
    public CompetitionStatus Status { get; private set; }
    public string? ImageUrl { get; private set; }
    public string ResultsToken { get; private set; } = string.Empty;

    public int OrganizerId { get; private set; }
    public virtual User Organizer { get; private set; } = null!;

    public int FisheryId { get; private set; }
    public virtual Fishery Fishery { get; private set; } = null!;

    private readonly List<CompetitionCategory> _categories = [];
    public virtual IReadOnlyCollection<CompetitionCategory> Categories => _categories.AsReadOnly();

    private readonly List<CompetitionParticipant> _participants = [];
    public virtual IReadOnlyCollection<CompetitionParticipant> Participants => _participants.AsReadOnly();

    private readonly List<CompetitionFishCatch> _fishCatches = [];
    public virtual IReadOnlyCollection<CompetitionFishCatch> FishCatches => _fishCatches.AsReadOnly();


    private Competition() { }

    public Competition(
        string name,
        DateTimeRange schedule,
        CompetitionType type,
        User organizer,
        Fishery fishery,
        string? rules = null,
        string? imageUrl = null)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.Null(schedule, nameof(schedule));
        Guard.Against.Null(organizer, nameof(organizer));
        Guard.Against.Null(fishery, nameof(fishery));

        Name = name;
        Schedule = schedule;
        Type = type;
        OrganizerId = organizer.Id;
        Organizer = organizer;
        FisheryId = fishery.Id;
        Fishery = fishery;
        Rules = rules;
        ImageUrl = imageUrl;
        Status = CompetitionStatus.Draft;
        ResultsToken = Guid.NewGuid().ToString("N");

        // Automatycznie dodaj organizatora jako uczestnika z rolą Organizator
        _participants.Add(new CompetitionParticipant(this, organizer, ParticipantRole.Organizer, true));
    }

    public void UpdateDetails(
        string name,
        DateTimeRange schedule,
        CompetitionType type,
        string? rules,
        string? imageUrl)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.Null(schedule, nameof(schedule));
        // TODO: Dodać walidację, czy można modyfikować (np. status Draft lub Upcoming)
        if (Status != CompetitionStatus.Draft && Status != CompetitionStatus.Upcoming && Status != CompetitionStatus.AcceptingRegistrations)
        {
            throw new InvalidOperationException("Szczegóły zawodów można zmieniać tylko w statusie Draft, Upcoming lub AcceptingRegistrations.");
        }

        Name = name;
        Schedule = schedule;
        Type = type;
        Rules = rules;
        ImageUrl = imageUrl;
    }

    // --- Zarządzanie Statusem ---
    public bool IsAcceptingRegistrations() => Status == CompetitionStatus.AcceptingRegistrations;
    public bool IsOngoing() => Status == CompetitionStatus.Ongoing && Schedule.Contains(DateTimeOffset.UtcNow);
    public bool IsFinished() => Status == CompetitionStatus.Finished;
    public bool CanModifyDetails() => Status == CompetitionStatus.Draft || Status == CompetitionStatus.AcceptingRegistrations || Status == CompetitionStatus.Scheduled;


    public void RequestApproval()
    {
        if (Status != CompetitionStatus.Draft)
            throw new InvalidOperationException("Wniosek o zatwierdzenie można złożyć tylko dla zawodów w wersji roboczej (Draft).");
        // TODO: Sprawdzić, czy wszystkie wymagane pola są uzupełnione (np. przynajmniej jedna kategoria główna)
        if (!_categories.Any(c => c.IsPrimaryScoring && c.IsEnabled))
            throw new InvalidOperationException("Zawody muszą mieć zdefiniowaną przynajmniej jedną aktywną kategorię główną przed zgłoszeniem do zatwierdzenia.");

        Status = CompetitionStatus.PendingApproval;
        // AddDomainEvent(new CompetitionApprovalRequestedEvent(this));
    }

    public void ApproveCompetition()
    {
        if (Status != CompetitionStatus.PendingApproval)
            throw new InvalidOperationException("Można zatwierdzić tylko zawody oczekujące na zatwierdzenie.");
        Status = CompetitionStatus.AcceptingRegistrations;
        // AddDomainEvent(new CompetitionApprovedEvent(this));
    }


    public void ScheduleCompetition()
    {
        if (Status != CompetitionStatus.AcceptingRegistrations)
            throw new InvalidOperationException("Można zaplanować tylko zawody, które akceptują zgłoszenia.");
        if (Schedule.Start <= DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Nie można zaplanować zawodów, których czas rozpoczęcia już minął.");
        Status = CompetitionStatus.Scheduled;
    }


    public void StartCompetition()
    {
        if (Status != CompetitionStatus.Scheduled && Status != CompetitionStatus.Upcoming) // Upcoming jeśli czas nadszedł
            throw new InvalidOperationException("Można rozpocząć tylko zaplanowane zawody.");
        if (Schedule.Start > DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Nie można rozpocząć zawodów przed zaplanowanym czasem rozpoczęcia.");
        if (Schedule.End <= DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Nie można rozpocząć zawodów, których czas zakończenia już minął.");

        Status = CompetitionStatus.Ongoing;
        // AddDomainEvent(new CompetitionStartedEvent(this));
    }

    public void FinishCompetition()
    {
        if (Status != CompetitionStatus.Ongoing)
            throw new InvalidOperationException("Można zakończyć tylko trwające zawody.");
        // Można dodać warunek, że czas zakończenia minął, lub pozwolić na wcześniejsze zakończenie
        // if (Schedule.End > DateTimeOffset.UtcNow)
        //    throw new InvalidOperationException("Nie można zakończyć zawodów przed zaplanowanym czasem zakończenia.");

        Status = CompetitionStatus.Finished;
        // AddDomainEvent(new CompetitionFinishedEvent(this));
    }

    public void CancelCompetition(string reason)
    {
        Guard.Against.NullOrWhiteSpace(reason, nameof(reason));
        if (Status == CompetitionStatus.Finished || Status == CompetitionStatus.Cancelled)
            throw new InvalidOperationException("Nie można anulować zakończonych lub już anulowanych zawodów.");

        Status = CompetitionStatus.Cancelled;
        // TODO: Zapisać powód anulowania, np. w nowym polu `CancellationReason`
        // AddDomainEvent(new CompetitionCancelledEvent(this, reason));
    }

    // --- Zarządzanie Uczestnikami ---
    public CompetitionParticipant AddParticipant(User user, ParticipantRole role, bool addedByOrganizer)
    {
        if (Status != CompetitionStatus.AcceptingRegistrations && Type == CompetitionType.Public && !addedByOrganizer)
            throw new InvalidOperationException("Nie można dołączyć do zawodów, które nie akceptują zgłoszeń.");
        if (Status != CompetitionStatus.AcceptingRegistrations && Status != CompetitionStatus.Draft && Status != CompetitionStatus.Scheduled && addedByOrganizer)
            throw new InvalidOperationException("Organizator może dodawać uczestników tylko gdy zawody są w statusie Draft, AcceptingRegistrations lub Scheduled.");


        if (_participants.Any(p => p.UserId == user.Id))
            throw new InvalidOperationException($"Użytkownik '{user.Name}' jest już uczestnikiem tych zawodów.");

        var participant = new CompetitionParticipant(this, user, role, addedByOrganizer);
        _participants.Add(participant);
        // AddDomainEvent(new ParticipantAddedToCompetitionEvent(this, participant));
        return participant;
    }

    public CompetitionParticipant AddGuestParticipant(string guestName, ParticipantRole role, bool addedByOrganizer, string? guestIdentifier = null)
    {
        if (Status != CompetitionStatus.AcceptingRegistrations && Status != CompetitionStatus.Draft && Status != CompetitionStatus.Scheduled)
            throw new InvalidOperationException("Gości można dodawać tylko gdy zawody są w statusie Draft, AcceptingRegistrations lub Scheduled.");

        if (role != ParticipantRole.Competitor && role != ParticipantRole.Guest)
            throw new ArgumentException("Gość może mieć tylko rolę Competitor lub Guest.", nameof(role));

        // Można dodać walidację unikalności GuestIdentifier, jeśli jest używany
        if (!string.IsNullOrWhiteSpace(guestIdentifier) && _participants.Any(p => p.GuestIdentifier == guestIdentifier))
            throw new InvalidOperationException($"Uczestnik-gość z identyfikatorem '{guestIdentifier}' już istnieje.");


        var guestParticipant = new CompetitionParticipant(this, guestName, role, addedByOrganizer, guestIdentifier);
        _participants.Add(guestParticipant);
        // AddDomainEvent(new ParticipantAddedToCompetitionEvent(this, guestParticipant));
        return guestParticipant;
    }

    public void RemoveParticipant(CompetitionParticipant participant)
    {
        Guard.Against.Null(participant, nameof(participant));
        if (Status != CompetitionStatus.Draft && Status != CompetitionStatus.AcceptingRegistrations && Status != CompetitionStatus.Scheduled)
            throw new InvalidOperationException("Uczestników można usuwać tylko przed rozpoczęciem zawodów (status Draft, AcceptingRegistrations, Scheduled).");

        if (participant.CompetitionId != Id)
            throw new ArgumentException("Uczestnik nie należy do tych zawodów.");

        if (participant.Role == ParticipantRole.Organizer && _participants.Count(p => p.Role == ParticipantRole.Organizer) == 1)
            throw new InvalidOperationException("Nie można usunąć jedynego organizatora zawodów.");

        // TODO: Rozważyć, co z połowami uczestnika, jeśli jakieś ma (choć przed startem nie powinien)
        _participants.Remove(participant);
        // AddDomainEvent(new ParticipantRemovedFromCompetitionEvent(this, participant));
    }

    public CompetitionParticipant AssignJudge(User user)
    {
        if (Status != CompetitionStatus.Draft && Status != CompetitionStatus.AcceptingRegistrations && Status != CompetitionStatus.Scheduled)
            throw new InvalidOperationException("Sędziów można przypisywać tylko przed rozpoczęciem zawodów.");

        var existingParticipant = _participants.FirstOrDefault(p => p.UserId == user.Id);
        if (existingParticipant != null)
        {
            if (existingParticipant.Role == ParticipantRole.Judge)
                throw new InvalidOperationException($"Użytkownik '{user.Name}' jest już sędzią w tych zawodach.");
            // Jeśli jest np. zawodnikiem, można rozważyć zmianę roli lub błąd
            existingParticipant.ChangeRole(ParticipantRole.Judge, Organizer); // Zakładając, że organizator to robi
            return existingParticipant;
        }

        var judgeParticipant = new CompetitionParticipant(this, user, ParticipantRole.Judge, true); // Sędzia dodany przez organizatora
        _participants.Add(judgeParticipant);
        // AddDomainEvent(new JudgeAssignedToCompetitionEvent(this, judgeParticipant));
        return judgeParticipant;
    }

    public void RemoveJudge(CompetitionParticipant judgeParticipant)
    {
        Guard.Against.Null(judgeParticipant, nameof(judgeParticipant));
        if (judgeParticipant.Role != ParticipantRole.Judge)
            throw new ArgumentException("Podany uczestnik nie jest sędzią.");
        if (Status != CompetitionStatus.Draft && Status != CompetitionStatus.AcceptingRegistrations && Status != CompetitionStatus.Scheduled)
            throw new InvalidOperationException("Sędziów można usuwać tylko przed rozpoczęciem zawodów.");

        // Można rozważyć, czy nie zmienić roli na "Competitor" zamiast usuwać, jeśli był też zawodnikiem
        _participants.Remove(judgeParticipant);
        // AddDomainEvent(new JudgeRemovedFromCompetitionEvent(this, judgeParticipant));
    }


    // --- Zarządzanie Kategoriami ---
    public CompetitionCategory AddCategory(
        CategoryDefinition categoryDefinition,
        bool isPrimaryScoring,
        int sortOrder = 0,
        int maxWinnersToDisplay = 1,
        int? fishSpeciesId = null,
        string? customNameOverride = null,
        string? customDescriptionOverride = null)
    {
        if (!CanModifyDetails())
            throw new InvalidOperationException("Kategorie można dodawać/modyfikować tylko gdy zawody pozwalają na edycję szczegółów.");

        if (isPrimaryScoring && _categories.Any(c => c.IsPrimaryScoring && c.IsEnabled))
        {
            throw new InvalidOperationException("Może istnieć tylko jedna aktywna główna kategoria punktacyjna.");
        }

        var competitionCategory = new CompetitionCategory(
            this, categoryDefinition, isPrimaryScoring, sortOrder, maxWinnersToDisplay,
            fishSpeciesId, customNameOverride, customDescriptionOverride);
        _categories.Add(competitionCategory);
        // AddDomainEvent(new CategoryAddedToCompetitionEvent(this, competitionCategory));
        return competitionCategory;
    }

    public void RemoveCategory(CompetitionCategory category)
    {
        Guard.Against.Null(category, nameof(category));
        if (!CanModifyDetails())
            throw new InvalidOperationException("Kategorie można usuwać tylko gdy zawody pozwalają na edycję szczegółów.");

        if (category.CompetitionId != Id)
            throw new ArgumentException("Kategoria nie należy do tych zawodów.");

        _categories.Remove(category);
        // AddDomainEvent(new CategoryRemovedFromCompetitionEvent(this, category));
    }

    // --- Zarządzanie Połowami ---
    public CompetitionFishCatch RecordFishCatch(
        CompetitionParticipant participant,
        User judge,
        FishSpecies fishSpecies,
        string imageUrl,
        DateTimeOffset catchTime,
        FishLength? length = null,
        FishWeight? weight = null)
    {
        if (!IsOngoing())
            throw new InvalidOperationException("Połowy można rejestrować tylko podczas trwających zawodów.");
        Guard.Against.Null(participant, nameof(participant));
        Guard.Against.Null(judge, nameof(judge));
        Guard.Against.Null(fishSpecies, nameof(fishSpecies));
        Guard.Against.NullOrWhiteSpace(imageUrl, nameof(imageUrl));

        if (participant.CompetitionId != Id)
            throw new ArgumentException("Uczestnik nie należy do tych zawodów.", nameof(participant));
        if (participant.Role != ParticipantRole.Competitor && participant.Role != ParticipantRole.Guest) // Gość też może być zawodnikiem
            throw new ArgumentException("Połów można zarejestrować tylko dla zawodnika lub gościa.", nameof(participant));

        var judgeAsParticipant = _participants.FirstOrDefault(p => p.UserId == judge.Id && p.Role == ParticipantRole.Judge);
        if (judgeAsParticipant == null)
            throw new InvalidOperationException($"Użytkownik '{judge.Name}' nie jest sędzią w tych zawodach lub nie ma uprawnień.");

        // Walidacja, czy przynajmniej jedna miara (długość/waga) jest podana, jeśli wymaga tego kategoria główna
        var primaryCategory = _categories.FirstOrDefault(c => c.IsPrimaryScoring && c.IsEnabled);
        if (primaryCategory != null)
        {
            var metric = primaryCategory.CategoryDefinition.Metric;
            if ((metric == CategoryMetric.LengthCm && length == null) ||
                (metric == CategoryMetric.WeightKg && weight == null))
            {
                // Można to złagodzić lub uczynić bardziej konfigurowalnym
                // throw new ArgumentException($"Dla głównej kategorii punktacyjnej ({metric}) wymagane jest podanie odpowiedniej miary (długość/waga).");
            }
        }
        if (length == null && weight == null)
        {
            // Można rozważyć, czy to zawsze błąd, czy np. liczy się tylko sztuka
            // throw new ArgumentException("Należy podać przynajmniej długość lub wagę ryby.");
        }


        var fishCatch = new CompetitionFishCatch(this, participant, judge, fishSpecies, imageUrl, catchTime, length, weight);
        _fishCatches.Add(fishCatch);
        // AddDomainEvent(new FishCatchRecordedEvent(this, fishCatch));
        return fishCatch;
    }
}
