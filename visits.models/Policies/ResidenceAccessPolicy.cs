using visits.models.Base;
using visits.models.Residences;

namespace visits.models.Policies;

public class ResidenceAccessPolicy: BaseEntity
{
    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; } = null!;
    public int DayOfWeek { get; set; }
    public TimeOnly AllowedFrom { get; set; }
    public TimeOnly AllowedUntil { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }
}