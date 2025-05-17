namespace Fishio.Domain.ValueObjects;

public class FishWeight : ValueObject
{
    public decimal Value { get; } // Waga w kilogramach
    public static string Unit => "kg";

    // Prywatny konstruktor dla EF Core, jeśli mapowany jako Owned Type
    private FishWeight() { }

    public FishWeight(decimal value)
    {
        Guard.Against.NegativeOrZero(value, nameof(value), $"Waga musi być wartością dodatnią. Podano: {value}");
        Guard.Against.OutOfRange(value, nameof(value), 0.001m, 200m, $"Waga poza dopuszczalnym zakresem (0.001 - 200 {Unit}). Podano: {value}"); // Przykładowy zakres
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => $"{Value} {Unit}";

    public static implicit operator decimal(FishWeight weight) => weight.Value;
    public static explicit operator FishWeight(decimal value) => new(value);
}
