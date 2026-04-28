using visits.models.Base;

namespace visits.models.Residences;

public class Residence:BaseTenant
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public int Capacity { get; set; }
    public Guid WardenUserId { get; set; }
    public BaseUser WardenUser { get; set; } = null!;
}