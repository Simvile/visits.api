using visits.models.Base;

namespace visits.models.Core;

public class Images:BaseEntity
{
    public string Name { get; set; } = null!;
    public string Size { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string Uri { get; set; } = null!;
}