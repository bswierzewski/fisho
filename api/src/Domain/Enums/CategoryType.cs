using System.ComponentModel;

namespace Fishio.Domain.Enums;

public enum CategoryType
{
    [Description("Główna kategoria punktacyjna")]
    MainScoring = 1,

    [Description("Specjalne osiągnięcie")]
    SpecialAchievement = 2,

    [Description("Kategoria rozrywkowa/dodatkowa")]
    FunChallenge = 3,

    [Description("Kategoria zdefiniowana przez użytkownika")]
    Custom = 4
}
