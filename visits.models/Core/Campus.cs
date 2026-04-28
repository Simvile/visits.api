using visits.models.Base;

namespace visits.models.Core;

public class Campus: BaseTenant
{
    public string Name { get; set; } = null!;
    public string? Code { get; set; } = null;
    public Address Address { get; set; } = null!;
    public Guid? CampusImageId { get; set; }
    public Images? CampusImage { get; set; }
}