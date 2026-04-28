using System.Security.Principal;
using visits.models.Base;
using visits.models.Core;
using visits.models.Residences;

namespace visits.models.Visitors;

public class Visits: BaseEntity
{
    public Guid VisitorId { get; set; }
    public Visitor Visitor { get; set; } = null!;
    public Guid HostUserId { get; set; }
    public BaseUser HostUser { get; set; } = null!;
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public Guid VisitTypeId { get; set; }
    public ClassificationValues VisitType { get; set; } = null!;
    public DateTime PlannedArrival { get; set; }
    public DateTime PlannedDeparture { get; set; }
    public DateTime? ActualArrival { get; set; }
    public DateTime? ActualDeparture { get; set; }
    public Guid StatusId { get; set; }
    public ClassificationValues Status { get; set; } = null!;
    public DateTime? ApprovedAt { get; set; }
    public bool HostConfirmedExit { get; set; }
    public string? Notes { get; set; }
}