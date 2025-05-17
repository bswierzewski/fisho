using System.ComponentModel;

namespace Fishio.Domain.Enums;

public enum CompetitionType
{
    [Description("Publiczny")]
    Public,
    [Description("Prywatny")]
    Private
}