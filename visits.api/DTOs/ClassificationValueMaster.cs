using visits.models.Base;

namespace visits.api.DTOs;

public class ClassificationValueMaster: BaseEntity
{
    public string Type { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Code { get; set; } = null!;
}