namespace Fishio.Domain.Events;

public class LogbookEntryCreatedEvent : BaseEvent
{
    public LogbookEntry LogbookEntry { get; }

    public LogbookEntryCreatedEvent(LogbookEntry logbookEntry)
    {
        LogbookEntry = logbookEntry;
    }
}
