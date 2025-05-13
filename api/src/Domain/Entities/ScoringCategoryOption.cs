using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class ScoringCategoryOption : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    
    // Navigation property
    public virtual ICollection<Competition> Competitions { get; private set; } = new List<Competition>();

    // Private constructor for EF Core
    private ScoringCategoryOption() { }

    public ScoringCategoryOption(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
