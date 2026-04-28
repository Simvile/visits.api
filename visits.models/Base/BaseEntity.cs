namespace visits.models.Base;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = null!;
    public DateTime UpdatedAt { get; set; }
}