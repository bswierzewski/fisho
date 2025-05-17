namespace Fishio.Application.Fisheries.Queries.GetFisheryDetails;

public record GetFisheryDetailsQuery : IRequest<FisheryDetailsDto>
{
    public int Id { get; init; }
}

public class GetFisheryDetailsQueryValidator : AbstractValidator<GetFisheryDetailsQuery>
{
    public GetFisheryDetailsQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id łowiska jest wymagane.");
    }
}