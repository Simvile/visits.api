using visits.models.Base;
using visits.models.Core;

namespace visits.models.Residences;

public class Room : BaseEntity
{
    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; } = null!;
    public string RoomNumber { get; set; } = null!;
    public Guid RoomTypeId { get; set; }
    public ClassificationValues RoomType { get; set; } = null!;
    public int MaxOccupants { get; set; }
}