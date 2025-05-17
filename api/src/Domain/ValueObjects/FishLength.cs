namespace Fishio.Domain.ValueObjects;

public class FishLength : ValueObject
{
    public decimal Value { get; } // Długość w centymetrach
    public static string Unit => "cm";

    // Prywatny konstruktor dla EF Core, jeśli mapowany jako Owned Type
    private FishLength() { }

    public FishLength(decimal value)
    {
        Guard.Against.NegativeOrZero(value, nameof(value), $"Długość musi być wartością dodatnią. Podano: {value}");
        Guard.Against.OutOfRange(value, nameof(value), 0.1m, 500m, $"Długość poza dopuszczalnym zakresem (0.1 - 500 {Unit}). Podano: {value}"); // Przykładowy zakres
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => $"{Value} {Unit}";

    public static implicit operator decimal(FishLength length) => length.Value;
    // Jawna konwersja może być bezpieczniejsza, aby uniknąć przypadkowego tworzenia
    public static explicit operator FishLength(decimal value) => new(value);
}
