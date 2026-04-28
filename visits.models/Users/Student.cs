using visits.models.Base;
using visits.models.Core;

namespace visits.models.Users;

public class Student : BaseEntity
{
    public Guid UserId { get; set; }
    public BaseUser User { get; set; } = null!;
    public Guid? GenderId { get; set; }
    public ClassificationValues? Gender { get; set; }
    public string? StudentNumber { get; set; }
}