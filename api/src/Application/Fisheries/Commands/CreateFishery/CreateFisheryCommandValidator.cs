using Fishio.Application.Fisheries.Commands.CreateFishery;

public class CreateFisheryCommandValidator : AbstractValidator<CreateFisheryCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateFisheryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Nazwa łowiska jest wymagana.")
            .MaximumLength(255).WithMessage("Nazwa łowiska nie może przekraczać 255 znaków.")
            .MustAsync(BeUniqueName).WithMessage("Łowisko o tej nazwie już istnieje.");

        RuleFor(v => v.Location)
            .MaximumLength(1000).WithMessage("Lokalizacja nie może przekraczać 1000 znaków.");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _context.Fisheries.AnyAsync(f => f.Name == name, cancellationToken);
    }
}