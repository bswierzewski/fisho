using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum MainScoringCategory
{
    [Display(Name = "Waga całkowita")]
    TotalWeight = 1,

    [Display(Name = "Długość całkowita")]
    TotalLength = 2,

    [Display(Name = "Punkt za sztukę")]
    PointsPerSpecies = 3
}
