namespace Fishio.Application.LookupData.Queries.GetListEnumValues;

public record GetListEnumValuesQuery(string EnumName) : IRequest<IEnumerable<EnumValueDto>>;

public class EnumValueDto
{
    public int Value { get; set; } // Wartość numeryczna enuma
    public string Name { get; set; } = string.Empty; // Nazwa tekstowa enuma
    public string? Description { get; set; } // Opcjonalny opis (można dodać atrybutami do enuma)
}

public class GetListEnumValuesQueryValidator : AbstractValidator<GetListEnumValuesQuery>
{
    public GetListEnumValuesQueryValidator()
    {
        RuleFor(x => x.EnumName)
            .NotEmpty().WithMessage("Nazwa enuma jest wymagana.");
    }
}