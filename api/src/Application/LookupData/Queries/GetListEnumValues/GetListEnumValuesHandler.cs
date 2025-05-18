using Application.Common.Extensions;

namespace Fishio.Application.LookupData.Queries.GetListEnumValues;

public class GetListEnumValuesHandler : IRequestHandler<GetListEnumValuesQuery, IEnumerable<EnumValueDto>>
{
    private static readonly Dictionary<string, Type> AllowedEnumTypes = new()
    {
        { nameof(CategoryType), typeof(CategoryType) },
        { nameof(CategoryMetric), typeof(CategoryMetric) },
        { nameof(CategoryCalculationLogic), typeof(CategoryCalculationLogic) },
        { nameof(CategoryEntityType), typeof(CategoryEntityType) },
        { nameof(CompetitionStatus), typeof(CompetitionStatus) },
        { nameof(CompetitionType), typeof(CompetitionType) },
        { nameof(ParticipantRole), typeof(ParticipantRole) }
        // { nameof(YourEnumName), typeof(YourEnumType) }
    };

    public Task<IEnumerable<EnumValueDto>> Handle(GetListEnumValuesQuery request, CancellationToken cancellationToken)
    {
        if (!AllowedEnumTypes.TryGetValue(request.EnumName, out var enumType))
            throw new NotFoundException(nameof(request.EnumName), $"Enum o nazwie '{request.EnumName}' nie został znaleziony lub nie jest dozwolony.");

        var enumValues = Enum.GetValues(enumType)
            .Cast<Enum>()
            .Select(e => new EnumValueDto
            {
                Value = Convert.ToInt32(e),
                Name = e.ToString(),
                Description = e.GetEnumDescription()
            })
            .OrderBy(e => e.Name)
            .ToList();

        return Task.FromResult<IEnumerable<EnumValueDto>>(enumValues);
    }
}
