namespace Fishio.Application.Competitions.Commands.AssignJudge;

public class AssignJudgeCommand : IRequest<int> // Zwraca ID wpisu CompetitionParticipant dla sędziego
{
    public int CompetitionId { get; set; }
    public int UserIdToAssignAsJudge { get; set; } // ID użytkownika (z encji User), który ma zostać sędzią
}

public class AssignJudgeCommandValidator : AbstractValidator<AssignJudgeCommand>
{
    private readonly IApplicationDbContext _context;

    public AssignJudgeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.CompetitionId)
            .NotEmpty().WithMessage("ID zawodów jest wymagane.");

        RuleFor(v => v.UserIdToAssignAsJudge)
            .NotEmpty().WithMessage("ID użytkownika do przypisania jako sędzia jest wymagane.")
            .GreaterThan(0).WithMessage("Nieprawidłowe ID użytkownika.")
            .MustAsync(UserMustExist).WithMessage("Wybrany użytkownik nie istnieje.");
        // Można dodać walidację, czy użytkownik nie jest już sędzią w tych zawodach,
        // ale metoda domenowa w Competition powinna to obsłużyć.
    }

    private async Task<bool> UserMustExist(int userId, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
    }
}
