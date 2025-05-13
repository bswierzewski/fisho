namespace Fishio.Application.Logbook.Commands.DeleteLogbookEntry;

public record DeleteLogbookEntryCommand : IRequest<Unit>
{
    public Guid LogbookEntryId { get; init; }
}

public class DeleteLogbookEntryCommandValidator : AbstractValidator<DeleteLogbookEntryCommand>
{
    public DeleteLogbookEntryCommandValidator()
    {
        RuleFor(x => x.LogbookEntryId)
            .NotEmpty().WithMessage("Logbook entry ID is required");
    }
} 