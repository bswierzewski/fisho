using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum ParticipantRole
    {
        [Display(Name = "Organizator")]
        Organizer,

        [Display(Name = "Sędzia")]
        Judge,

        [Display(Name = "Uczestnik")]
        Participant
    }
}
