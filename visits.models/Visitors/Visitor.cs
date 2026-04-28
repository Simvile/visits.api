using visits.models.Base;
using visits.models.Core;

namespace visits.models.Visitors;

public class Visitor:BaseEntity
{
    public string FullName { get; set; } = null!;
    public string? IdNumber { get; set; } = null;
    public string? StudentNumber { get; set; } = null;
    public Guid VisitationTypeId { get; set; }
    public ClassificationValues VisitationType { get; set; } = null!;
}