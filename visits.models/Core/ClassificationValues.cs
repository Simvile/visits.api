using visits.models.Base;

namespace visits.models.Core;

public class ClassificationValues: BaseEntity
{
    public string Type { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Code { get; set; } = null!;
}