using System.ComponentModel;
using System.Reflection;
using Ardalis.GuardClauses;

namespace Fishio.Application.LookupData.Queries.ListEnumValues;

public class ListEnumValuesQueryHandler : IRequestHandler<ListEnumValuesQuery, IEnumerable<EnumValueDto>>
{
    // Można wstrzyknąć listę dozwolonych typów enumów, jeśli chcemy ograniczyć dostęp
    private static readonly Dictionary<string, Type> AllowedEnumTypes = new()
    {
        { nameof(CategoryType), typeof(CategoryType) },
        { nameof(CategoryMetric), typeof(CategoryMetric) },
        { nameof(CategoryCalculationLogic), typeof(CategoryCalculationLogic) },
        { nameof(CategoryEntityType), typeof(CategoryEntityType) },
        { nameof(CompetitionStatus), typeof(CompetitionStatus) },
        { nameof(CompetitionType), typeof(CompetitionType) },
        { nameof(ParticipantRole), typeof(ParticipantRole) }
        // Dodaj tutaj inne enumy, które chcesz udostępnić
    };

    public Task<IEnumerable<EnumValueDto>> Handle(ListEnumValuesQuery request, CancellationToken cancellationToken)
    {
        if (!AllowedEnumTypes.TryGetValue(request.EnumName, out var enumType))
            throw new NotFoundException(nameof(request.EnumName), $"Enum o nazwie '{request.EnumName}' nie został znaleziony lub nie jest dozwolony.");

        var enumValues = Enum.GetValues(enumType)
            .Cast<Enum>()
            .Select(e => new EnumValueDto
            {
                Id = Convert.ToInt32(e),
                Name = e.ToString(),
                Description = GetEnumDescription(e)
            })
            .OrderBy(e => e.Name) // Opcjonalnie sortuj
            .ToList();

        return Task.FromResult<IEnumerable<EnumValueDto>>(enumValues);
    }

    private static string? GetEnumDescription(Enum value)
    {
        FieldInfo? fi = value.GetType().GetField(value.ToString());

        if (fi != null)
        {
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
        }
        return value.ToString(); // Lub null, jeśli brak opisu
    }
}
