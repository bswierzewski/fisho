namespace Fishio.Application.LookupData.Queries.ListEnumValues;

public record ListEnumValuesQuery(string EnumName) : IRequest<IEnumerable<EnumValueDto>>;

public class EnumValueDto
{
    public int Id { get; set; } // Wartość numeryczna enuma
    public required string Name { get; set; } // Nazwa tekstowa enuma
    public string? Description { get; set; } // Opcjonalny opis (można dodać atrybutami do enuma)
}

public class ListEnumValuesQueryValidator : AbstractValidator<ListEnumValuesQuery>
{
    public ListEnumValuesQueryValidator()
    {
        RuleFor(x => x.EnumName)
            .NotEmpty().WithMessage("Nazwa enuma jest wymagana.");
    }
}