using visits.models.Base;

namespace visits.api.DTOs;

public class AddressMaster : BaseEntity
{
    public string Street { get; set; } = null!;
    public string? Complex { get; set; } = null;
    public string? City { get; set; } = null;
    public string? Province { get; set; } = null;
    public string? PostalCode { get; set; } = null;
}