using visits.models.Base;

namespace visits.models.Core;

public class Institution : BaseEntity
{
    public Guid TypeId { get; set; }
    public ClassificationValues Type { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string LogoUrl { get; set; } = null!;
    public Guid AddressId { get; set; }
    public Address Address { get; set; } = null!;
}