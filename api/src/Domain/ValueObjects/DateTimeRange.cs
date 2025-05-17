namespace Fishio.Domain.ValueObjects;

public class DateTimeRange : ValueObject
{
    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }

    // Prywatny konstruktor dla EF Core
    private DateTimeRange() { }

    public DateTimeRange(DateTimeOffset start, DateTimeOffset end)
    {
        Guard.Against.Default(start, nameof(start));
        Guard.Against.Default(end, nameof(end));

        if (start >= end)
        {
            throw new ArgumentException("Czas rozpoczęcia musi być wcześniejszy niż czas zakończenia.");
        }

        Start = start;
        End = end;
    }

    public TimeSpan Duration => End - Start;

    public bool Contains(DateTimeOffset dateTimeOffset)
    {
        return dateTimeOffset >= Start && dateTimeOffset <= End;
    }

    public bool Overlaps(DateTimeRange otherRange)
    {
        return Start < otherRange.End && End > otherRange.Start;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }

    public override string ToString() => $"Od: {Start} Do: {End}";
}
