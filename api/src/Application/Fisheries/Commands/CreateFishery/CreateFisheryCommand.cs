using Microsoft.AspNetCore.Http;

namespace Fishio.Application.Fisheries.Commands.CreateFishery;

public record CreateFisheryCommand : IRequest<int>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public IFormFile? Image { get; set; }
    public List<int> FishSpeciesIds { get; init; } = new();
}

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

        RuleForEach(x => x.FishSpeciesIds)
                    .GreaterThan(0).When(x => x != null).WithMessage("Nieprawidłowe ID gatunku ryby.");

        When(x => x.Image != null, () =>
        {
            RuleFor(x => x.Image)
                .Must(BeAValidImage).WithMessage("Nieprawidłowy format zdjęcia lub za duży plik (max 5MB).");
        });
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _context.Fisheries.AnyAsync(f => f.Name == name, cancellationToken);
    }

    private bool BeAValidImage(IFormFile? file)
    {
        if (file == null) return true;
        if (file.Length == 0) return true;

        if (file.Length > 5 * 1024 * 1024) return false;

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        return allowedTypes.Contains(file.ContentType.ToLower());
    }
}