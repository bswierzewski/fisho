namespace Fishio.Application.Fisheries.Commands.DeleteFishery;

public record DeleteFisheryCommand(int Id) : IRequest<Unit>;

public class DeleteFisheryCommandValidator : AbstractValidator<DeleteFisheryCommand>
{
    public DeleteFisheryCommandValidator()
    {
        RuleFor(v => v.Id).GreaterThan(0);
    }
}
