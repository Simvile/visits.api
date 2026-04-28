using visits.models.Base;

namespace visits.models.Core;

public class Address: BaseEntity
{
    public string Street { get; set; } = null!;
    public string? Complex { get; set; } = null;
    public string? City { get; set; } = null;
    public string? Province { get; set; } = null;
    public string? PostalCode { get; set; } = null;
}