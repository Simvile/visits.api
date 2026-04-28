using visits.models.Base;
using visits.models.Core;

namespace visits.models.Policies;

public class VisitTypePolicy :BaseEntity
{
    public Guid PolicyId { get; set; }
    public ResidenceAccessPolicy Policy { get; set; } = null!;
    
    public Guid VisitTypeId { get; set; }
    public ClassificationValues VisitType { get; set; } = null!;
    
    public bool IsAllowed { get; set; }
    public int MaxOvernightDays { get; set; }
}