using System.ComponentModel;

namespace Fishio.Domain.Enums;

public enum CompetitionStatus
{
    [Description("Wersja robocza")]
    Draft,

    [Description("W trakcie zatwierdzania")]
    PendingApproval,

    [Description("Zaplanowane")]
    Upcoming,

    [Description("Trwające")]
    Ongoing,

    [Description("Zakończone")]
    Finished
}
