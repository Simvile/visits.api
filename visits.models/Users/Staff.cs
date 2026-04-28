using visits.models.Base;

namespace visits.models.Users;

public class Staff: BaseEntity
{
    public Guid UserId { get; set; }
    public BaseUser User { get; set; } = null!;
    public string IdentityNumber { get; set; } = null!;
    public DateTime LastLogged { get; set; }
}