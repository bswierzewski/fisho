using FluentValidation;

namespace Fishio.Application.Fisheries.Commands.DeleteFishery;

public class DeleteFisheryCommandValidator : AbstractValidator<DeleteFisheryCommand>
{
    public DeleteFisheryCommandValidator()
    {
        RuleFor(v => v.Id).GreaterThan(0);
    }
}
