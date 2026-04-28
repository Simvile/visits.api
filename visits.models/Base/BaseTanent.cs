using visits.models.Core;

namespace visits.models.Base;

public abstract class BaseTenant: BaseEntity
{
    public Guid InstitutionId { get; set; }
}