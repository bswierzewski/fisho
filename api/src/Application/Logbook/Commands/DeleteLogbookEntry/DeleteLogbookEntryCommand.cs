namespace Fishio.Application.Logbook.Commands.DeleteLogbookEntry;

public record DeleteLogbookEntryCommand : IRequest<Unit>
{
    public int Id { get; init; }
}

public class DeleteLogbookEntryCommandValidator : AbstractValidator<DeleteLogbookEntryCommand>
{
    public DeleteLogbookEntryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Logbook entry ID is required");
    }
} 