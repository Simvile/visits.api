using visits.models.Base;

namespace visits.models.Residences;

public class StudentRoom : BaseEntity
{
    public Guid UserId { get; set; }
    public BaseUser User { get; set; } = null!;
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public DateTime AssignedAt { get; set; }
    public DateTime? VacatedAt { get; set; }
}