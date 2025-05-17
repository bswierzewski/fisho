using Fishio.Application.Fisheries.Commands.UpdateFishery;

public class UpdateFisheryCommandValidator : AbstractValidator<UpdateFisheryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateFisheryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id).GreaterThan(0);

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Nazwa łowiska jest wymagana.")
            .MaximumLength(255).WithMessage("Nazwa łowiska nie może przekraczać 255 znaków.");

        // Sprawdzenie unikalności nazwy, ignorując aktualnie edytowane łowisko
        RuleFor(v => v)
            .MustAsync(async (command, cancellation) =>
            {
                return !await _context.Fisheries
                    .AnyAsync(f => f.Name == command.Name && f.Id != command.Id, cancellation);
            })
            .WithMessage("Łowisko o tej nazwie już istnieje.")
            .WithName("Name"); // Aby błąd był przypisany do pola Name
    }
}