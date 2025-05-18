namespace Fishio.Application.LogbookEntries.Commands.DeleteLogbookEntry;

public class DeleteLogbookEntryCommand : IRequest<bool> // Zwraca bool wskazujący sukces
{
    public int Id { get; set; }

    public DeleteLogbookEntryCommand(int id)
    {
        Id = id;
    }
}

public class DeleteLogbookEntryCommandValidator : AbstractValidator<DeleteLogbookEntryCommand>
{
    public DeleteLogbookEntryCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("ID wpisu jest wymagane.");
    }
}
