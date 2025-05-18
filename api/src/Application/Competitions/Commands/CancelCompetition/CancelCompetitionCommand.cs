namespace Fishio.Application.Competitions.Commands.CancelCompetition;

public class CancelCompetitionCommand : IRequest<bool>
{
    public int CompetitionId { get; set; }
    public string Reason { get; set; } = "Przyczyny nie zostały podane."; // Domyślny powód
}

public class CancelCompetitionCommandValidator : AbstractValidator<CancelCompetitionCommand>
{
    public CancelCompetitionCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.CompetitionId)
            .NotEmpty().WithMessage("ID zawodów jest wymagane.")
            .MustAsync(async (id, ct) =>
            {
                var competition = await context.Competitions
                    .AsNoTracking()
                    .Select(c => new { c.Id, c.Status })
                    .FirstOrDefaultAsync(c => c.Id == id, ct);
                // Nie można anulować zakończonych lub już anulowanych
                return competition != null &&
                       competition.Status != CompetitionStatus.Finished &&
                       competition.Status != CompetitionStatus.Cancelled;
            }).WithMessage("Zawody nie istnieją, są już zakończone lub anulowane.");

        RuleFor(v => v.Reason)
            .NotEmpty().WithMessage("Powód anulowania jest wymagany.")
            .MaximumLength(500).WithMessage("Powód anulowania nie może przekraczać 500 znaków.");
    }
}
