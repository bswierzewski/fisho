namespace Domain.Entities
{
    public class SpecialCategoryOption : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
