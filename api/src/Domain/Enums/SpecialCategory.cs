using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum SpecialCategory
{
    [Display(Name = "Brak")]
    None = 0,
    [Display(Name = "Największa ryba")]
    BiggestWeight = 1,
    [Display(Name = "Najdłuższa ryba")]
    LongestFish = 2,
    [Display(Name = "Ilość złapanych ryb")]
    MostCatchesCount = 3
}
