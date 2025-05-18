namespace Fishio.Application.Competitions.Commands.FinishCompetition;

public class FinishCompetitionCommand : IRequest<bool>
{
    public int CompetitionId { get; set; }
}

public class FinishCompetitionCommandValidator : AbstractValidator<FinishCompetitionCommand>
{
    public FinishCompetitionCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.CompetitionId)
            .NotEmpty().WithMessage("ID zawodów jest wymagane.")
            .MustAsync(async (id, ct) =>
            {
                var competition = await context.Competitions
                    .AsNoTracking()
                    .Select(c => new { c.Id, c.Status })
                    .FirstOrDefaultAsync(c => c.Id == id, ct);
                return competition != null && competition.Status == CompetitionStatus.Ongoing;
            }).WithMessage("Zawody nie istnieją lub nie są w trakcie (Ongoing).");
    }
}
