namespace Fishio.Application.Fisheries.Commands.DeleteFishery;

public class DeleteFisheryCommand : IRequest<bool> // Zwraca bool wskazujący sukces
{
    public int Id { get; set; }

    public DeleteFisheryCommand(int id)
    {
        Id = id;
    }
}

public class DeleteFisheryCommandValidator : AbstractValidator<DeleteFisheryCommand>
{
    public DeleteFisheryCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("ID łowiska jest wymagane.");
    }
}
